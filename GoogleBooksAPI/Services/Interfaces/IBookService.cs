using GoogleBooksAPI.Models.GoogleAPI;
using System.Collections.Generic;
using GoogleBooksAPI.Models.Dto;
using System.Threading.Tasks;
using System;

namespace GoogleBooksAPI.Services.Interfaces
{
    public interface IBookService
    {
        #region Add New
        Task AddNewBook(VolumeInfo model);
        #endregion

        #region Get Books - List / IEnumerable

        #region Returns result on Google Library API stored localy - EF using linq
        Task<List<VolumeInfo>> GetBooks_ef_google_search_method(Guid searchHistoryGuidId);
        #endregion

        #region Returns search result on Local Library based on Search Params - EF using linq
        Task<List<VolumeInfo>> GetBooks_ef_local_search_method(SearchQueryDto model);
        #endregion

        #region Returns search result on Local Library based on Search Params - Dapper using SP
        Task<DapperResult<IEnumerable<VolumeInfoDto_dapper>>> GetBooks_dapper(SearchQueryDto model);
        #endregion

        #endregion

        #region Get Book by bookid (Google Library API Unique id for books)
        Task<VolumeInfo> GetBookById(Guid bookid);
        #endregion

        #region Get / Check if exists Volume (Google API Librart API class name for Books) on Local DB
        Task<bool> VolumeExistsByItemId(string id);
        Task<VolumeInfo> GetVolumeByItemId(string id);

        #endregion
    }
}
