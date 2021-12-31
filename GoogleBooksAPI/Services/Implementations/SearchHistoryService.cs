using GoogleBooksAPI.Models.SearchModels;
using GoogleBooksAPI.Services.Interfaces;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Data.Entity;
using GoogleBooksAPI.Db;
using System;

namespace GoogleBooksAPI.Services.Implementations
{
    public class SearchHistoryService : ISearchHistoryService
    {
        #region ctor
        public SearchHistoryService()
        {
            // Make sure Migrations Applied and DB exists
            using (var context = new HAWClientDbContext())
                context.Database.Initialize(false);
        }
        #endregion

        #region Add / Update Search Query Result (Search query & Search Result Mapper)
        public async Task AddSearchQueryResults(SearchQueryResultModel model)
        {
            using (var context = new HAWClientDbContext())
            {
                context.SearchQueryResults.Add(model);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddOrUpdateSearchQueryResults(SearchQueryResultModel model)
        {
            using (var context = new HAWClientDbContext())
            {
                context.SearchQueryResults.AddOrUpdate(model);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Search Histories - Search Result Stored in DB

        #region Add / Update
        public async Task AddSearchHistories(SearchHistoryModel model)
        {
            using (var context = new HAWClientDbContext())
            {
                context.SearchHistories.Add(model);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddOrUpdateSearchHistories(SearchHistoryModel model)
        {
            using (var context = new HAWClientDbContext())
            {
                context.SearchHistories.AddOrUpdate(model);
                await context.SaveChangesAsync();
            }
        }
        #endregion

        #region Get By user search query
        public async Task<SearchHistoryModel> GetSearchHistoryModelBySearchQuery(string query)
        {
            using (var context = new HAWClientDbContext())
            {
                return await context.SearchHistories.FirstOrDefaultAsync(x => x.SearchQuery == query);
            }
        }
        #endregion

        #region Get By user search query made during last (param: min) minutes
        public async Task<SearchHistoryModel> GetSearchHistoryModelBySearchQueryWithinNmin(string query, int min)
        {
            using (var context = new HAWClientDbContext())
            {
                return await context.SearchHistories.FirstOrDefaultAsync(x => x.SearchQuery == query && x.ModifiedDateTime >= DbFunctions.AddMinutes(DateTime.UtcNow, -1 * min));
            }
        }
        #endregion

        #region Check if user search query exists (user'd searched the query before)
        public async Task<bool> SearchHistoryExists(string query)
        {
            using (var context = new HAWClientDbContext())
            {
                return await context.SearchHistories.AnyAsync(x => x.SearchQuery == query);
            }
        }  
        #endregion

        #endregion

    }
}