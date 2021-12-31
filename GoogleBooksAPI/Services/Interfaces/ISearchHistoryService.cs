using GoogleBooksAPI.Models.SearchModels;
using System.Threading.Tasks;

namespace GoogleBooksAPI.Services.Interfaces
{
    public interface ISearchHistoryService
    {
        #region Add / Update Search Query Result (Search query & Search Result Mapper)
        Task AddSearchQueryResults(SearchQueryResultModel model);

        Task AddOrUpdateSearchQueryResults(SearchQueryResultModel model);
        #endregion

        #region Search Histories - Search Result Stored in DB

        #region Add / Update
        Task AddSearchHistories(SearchHistoryModel model);

        Task AddOrUpdateSearchHistories(SearchHistoryModel model);
        #endregion

        #region Get By user search query
        Task<SearchHistoryModel> GetSearchHistoryModelBySearchQuery(string query);
        #endregion

        #region Get By user search query made during last (param: min)
        Task<SearchHistoryModel> GetSearchHistoryModelBySearchQueryWithinNmin(string query, int min);
        #endregion

        #region Check if user search query exists (user'd searched the query before)
        Task<bool> SearchHistoryExists(string query);
        #endregion

        #endregion
    }
}
