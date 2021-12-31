using GoogleBooksAPI.Services.Interfaces;
using GoogleBooksAPI.Models.GoogleAPI;
using System.Collections.Generic;
using GoogleBooksAPI.Models.Dto;
using GoogleBooksAPI.Utilities;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;
using GoogleBooksAPI.Db;
using System.Data;
using System.Linq;
using Dapper;
using System;

namespace GoogleBooksAPI.Services.Implementations
{
    public class BookService : IBookService
    {
        #region Ctor
        public BookService()
        {
            // Make sure Migrations Applied and DB exists
            using (var context = new HAWClientDbContext())
                context.Database.Initialize(false);
        }
        #endregion

        #region Add New
        public async Task AddNewBook(VolumeInfo model)
        {
            using (var context = new HAWClientDbContext())
            {
                context.VolumeInfos.Add(model);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Get Book-  List with Cache
        public async Task<DapperResult<List<VolumeInfo>>> FilterAndSortCachedBookSearch(SearchQueryDto model)
        {
            DapperResult<List<VolumeInfo>> result = new DapperResult<List<VolumeInfo>>();
            result.Data = CacheHelper.Get<List<VolumeInfo>>("google_api_memory_cache");
            if (result.Data is null || !result.Data.Any())
            {
                result.Data = await new BookService().GetBooks_all();
                CacheHelper.Set("google_api_memory_cache", result.Data, DateTime.UtcNow.AddMinutes(5));
            }

            result.Data = result.Data.Where(x =>
                                            (string.IsNullOrWhiteSpace(model.Intitle) || (x.Title != null && x.Title.ToLower().Contains(model.Intitle.ToLower())))
                                        && (string.IsNullOrWhiteSpace(model.Inauthor) || (x.AuthorList != null && x.AuthorList.ToLower().Contains(model.Inauthor.ToLower())))
                                        && (string.IsNullOrWhiteSpace(model.Inpublisher) || (x.Publisher != null && x.Publisher.ToLower().Contains(model.Inpublisher.ToLower())))
                                        && (string.IsNullOrWhiteSpace(model.Indescription) || (x.Description != null && x.Description.ToLower().Contains(model.Indescription.ToLower())))
                            ).OrderBy(x => x.Title).ToList();


            result.PageItemCount = 40;
            result.PageNumber = model.StartIndex;
            result.TotalRows = result.Data.Count;

            result.Data = result.Data
                           .Skip(40 * (result.PageNumber - 1))
                           .Take(40)
                           .ToList();

            return result;
        } 
        #endregion

        #region Get Books - List / IEnumerable

        #region Returns all books - EF
        public async Task<List<VolumeInfo>> GetBooks_all()
        {
            using (var context = new HAWClientDbContext())
            {

                return await context
                                .VolumeInfos
                                .Include(x => x.ImageLinks)
                                .Include(x => x.ReadingModes)
                                .ToListAsync();
            }
        }
        #endregion

        #region Returns result on Google Library API stored localy - EF using linq
        public async Task<List<VolumeInfo>> GetBooks_ef_google_search_method(Guid searchHistoryGuidId)
        {
            using (var context = new HAWClientDbContext())
            {

                return await context
                                .VolumeInfos
                                .Include(x => x.ImageLinks)
                                .Include(x => x.ReadingModes)
                                .Where(x => x.SearchGuid == searchHistoryGuidId)
                                .ToListAsync();
            }
        }
        #endregion

        #region Returns search result on Local Library based on Search Params - EF using linq
        public async Task<List<VolumeInfo>> GetBooks_ef_local_search_method(SearchQueryDto model)
        {
            using (var context = new HAWClientDbContext())
            {
                return await context
                            .VolumeInfos
                            .Include(q => q.ImageLinks)
                            .Include(q => q.ReadingModes)
                            .Where(x => (string.IsNullOrEmpty(model.Intitle) || x.Title.Contains(model.Intitle))
                                     && (string.IsNullOrEmpty(model.Inauthor) || x.AuthorList.Contains(model.Inauthor))
                                     && (string.IsNullOrEmpty(model.Inpublisher) || x.Publisher.Contains(model.Inpublisher))
                                     && (string.IsNullOrEmpty(model.Indescription) || x.Description.Contains(model.Indescription))
                            )
                            .OrderByDescending(x => x.Title)
                            .Skip(model.PageSize * (model.PageNumber))
                            .Take(model.PageSize)
                            .ToListAsync();
            }
        }
        #endregion

        #region Returns search result on Local Library based on Search Params - Dapper using SP
        public async Task<DapperResult<IEnumerable<VolumeInfoDto_dapper>>> GetBooks_dapper(SearchQueryDto model)
        {

            string sp = "dbo.stp_books_get";
            DynamicParameters param = new DynamicParameters();

            param.Add("@pTitle", model.Intitle);
            param.Add("@pAuthor", model.Inauthor);
            param.Add("@pPublisher", model.Inpublisher);
            param.Add("@pDescription", model.Indescription);
            param.Add("@pPageNumber", model.StartIndex);
            param.Add("@pTotalRows", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (IDbConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["HAWClientConnectionString"].ConnectionString))
            {

                DapperResult<IEnumerable<VolumeInfoDto_dapper>> result = new DapperResult<IEnumerable<VolumeInfoDto_dapper>>();
                try
                {
                    result.Data = await connection.QueryAsync<VolumeInfoDto_dapper>(sp, param, commandType: CommandType.StoredProcedure);
                    result.IsSuccess = true;
                    result.TotalRows = param.Get<int>("@pTotalRows");
                }
                catch (Exception e)
                {
                    result.Message = e.Message;
                    result.IsSuccess = false;
                }
                return result;
            }
        }
        #endregion

        #endregion

        #region Get Book by bookid (Google Library API Unique id for books)
        public async Task<VolumeInfo> GetBookById(Guid bookid)
        {
            using (var context = new HAWClientDbContext())
            {
                return await context
                                .VolumeInfos
                                .Include(p => p.ImageLinks)
                                .Include(p => p.ReadingModes)
                                .FirstOrDefaultAsync(x => x.Entity_Id == bookid);
            }
        }
        #endregion

        #region Get / Check if exists Volume (Google API Librart API class name for Books) on Local DB
        public async Task<bool> VolumeExistsByItemId(string id)
        {
            using (var context = new HAWClientDbContext())
            {
                return await context.VolumeInfos.AnyAsync(x => x.Item_id == id);
            }
        }

        public async Task<VolumeInfo> GetVolumeByItemId(string id)
        {
            using (var context = new HAWClientDbContext())
            {
                return await context.VolumeInfos.Include(p => p.ImageLinks).Include(p => p.ReadingModes).FirstOrDefaultAsync(x => x.Item_id == id);
            }
        }
        #endregion
    }
}