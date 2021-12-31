using GoogleBooksAPI.Services.Implementations;
using GoogleBooksAPI.Services.Interfaces;
using GoogleBooksAPI.Models.SearchModels;
using GoogleBooksAPI.Services.ApiHelper;
using GoogleBooksAPI.Models.GoogleAPI;
using System.Collections.Generic;
using GoogleBooksAPI.Models.Dto;
using GoogleBooksAPI.Utilities;
using System.Threading.Tasks;
using GoogleBooksAPI.Enums;
using System.Web.Mvc;
using System.Net;
using System;

namespace GoogleBooksAPI.Controllers
{
    public class HomeController : Controller
    {
        #region Ctor and Vars
        private const int MIN_DIFF_TO_UPDATE = 5;
        private readonly IBookService _bookService;
        private readonly ISearchHistoryService _searchHistoryService;
        public HomeController()
        {
            ApiHelperService.InitializeClient();
            _bookService = new BookService();
            _searchHistoryService = new SearchHistoryService();
        }
        #endregion

        #region Index Page - Live Library (Google Library API)
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View();
        }
        #endregion

        #region Search Methods - EndPoint Entry Get Method

        /// <summary>
        /// A GET Method which allows user to search based on params on Google Library API
        /// This function concat exisitng filters to create a query to make HTTP GET request to the Google Library API
        /// 
        /// SearchTypes:
        /// a) Type = 0 (NotForced)
        ///     When user requests Not Forced search, HTTP request will only made to the API if same query hasn't been made before.
        ///     if exists a previous request with the same search query, stored result of the search query will be sent to client
        ///     else a new HTTP request will be made and after storing the result, it will be sent to client
        /// b) Type = 1 (LiveForced)
        ///     When user requests Not Forced search, an HTTP request will be made to the API regardless of the search histories
        ///     returned result will be stored (override duplicate values) and it will be sent to client
        /// c) Type = 2 (FiveMinForced)
        ///     When user requests Not Forced search, HTTP request will only made to the API if same query hasn't been made before.
        ///     if exists a previous request with the same search query that has been made "DURING LAST 5 MINS", stored result of the search query will be sent to client
        ///     else a new HTTP request will be made and after storing the result, it will be sent to client
        /// </summary>
        /// <param name="intitle">Title filter (string)</param>
        /// <param name="inauthor">Author name filter (string)</param>
        /// <param name="inpublisher">Publisher name filter (string)</param>
        /// <param name="indescription">Text contains in description (string)</param>
        /// <param name="startIndex">Pagination param for Google Library API</param>
        /// <param name="type">Type of search (Which search Button clicked) - Not Forced | Live Forced | 5min Forced</param>
        /// <returns>List of Books</returns>
        [HttpGet]
        public async Task<ActionResult> q(string intitle, string inauthor, string inpublisher, string indescription, int startIndex, searchType type = searchType.NotForced)
        {
            string searchQuery = String.Empty;
            if (!String.IsNullOrWhiteSpace(intitle))
                searchQuery = Utils.addParamToSearchQuery(searchQuery, nameof(intitle), intitle);

            if (!String.IsNullOrWhiteSpace(inauthor))
                searchQuery = Utils.addParamToSearchQuery(searchQuery, nameof(inauthor), inauthor);

            if (!String.IsNullOrWhiteSpace(inpublisher))
                searchQuery = Utils.addParamToSearchQuery(searchQuery, nameof(inpublisher), inpublisher);

            if (!String.IsNullOrWhiteSpace(indescription))
                searchQuery = Utils.addParamToSearchQuery(searchQuery, nameof(indescription), indescription);

            if (String.IsNullOrWhiteSpace(searchQuery))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "At least one search parameter is required!");

            searchQuery = Utils.addParamToSearchQuery(searchQuery, nameof(startIndex), startIndex.ToString());

            switch (type)
            {
                case searchType.NotForced:
                    return Json(await SearchBooks(searchQuery), JsonRequestBehavior.AllowGet);

                case searchType.LiveForced:
                    return Json(await SearchBooks_ForceLive(searchQuery), JsonRequestBehavior.AllowGet);

                case searchType.FiveMinForced:
                    return Json(await SearchBooksWithinNmin(searchQuery), JsonRequestBehavior.AllowGet);

                default:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "At least one search parameter is required!");
            }

        }
        #endregion

        #region private methods - EF - Search / Add / Update

        /// <summary>
        /// This function will be called when SearchTypes is NotForced
        /// First, it will check if the same search (query) has been made before and will return stored result of the search query to client if it's true
        /// else, a new HTTP request will be send to Google Library API and after storing the result, it will be sent to client
        /// </summary>
        /// <param name="query">Concatenated user search query as string</param>
        /// <returns>List of books in Json format</returns>
        private async Task<ActionResult> SearchBooks(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "At least one search parameter is required!");


            List<VolumeInfo> books;
            try
            {
                SearchHistoryModel newSearch = await _searchHistoryService.GetSearchHistoryModelBySearchQuery(query);
                if (newSearch != null)
                {
                    books = await _bookService.GetBooks_ef_google_search_method(newSearch.Entity_Id);
                }
                else
                {
                    bool newVolumeAdded = false;
                    newSearch = new SearchHistoryModel { Entity_Id = Guid.NewGuid(), SearchQuery = query };

                    books = new List<VolumeInfo>();

                    var items = await FetchBooksFromAPI(query);

                    if (items != null)
                    {
                        foreach (Item item in items)
                        {
                            if (!await _bookService.VolumeExistsByItemId(item.Id))
                            {
                                if (item.VolumeInfo.Authors != null)
                                    item.VolumeInfo.AuthorList = String.Join(", ", item.VolumeInfo.Authors);
                                if (item.VolumeInfo.Categories != null)
                                    item.VolumeInfo.CategoryList = String.Join(", ", item.VolumeInfo.Categories);

                                item.VolumeInfo.Item_id = item.Id;
                                item.VolumeInfo.SearchGuid = newSearch.Entity_Id;

                                newSearch.VoloumeInfo.Add(item.VolumeInfo);
                                newVolumeAdded = true;
                            }

                            books.Add(item.VolumeInfo);
                        }

                        if (newVolumeAdded)
                        {
                            await _searchHistoryService.AddSearchHistories(newSearch);

                            foreach (var item in newSearch.VoloumeInfo)
                            {
                                await _searchHistoryService.AddSearchQueryResults(new SearchQueryResultModel()
                                {
                                    SearchId = newSearch.Entity_Id,
                                    VolumeInfoId = item.Entity_Id
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(books, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This function will be called when SearchTypes is LiveForced
        /// It will send a new HTTP request to Google Library API, stors the returned result (AddOrUpdate) and send it to client
        /// </summary>
        /// <param name="query">Concatenated user search query as string</param>
        /// <returns>List of books in Json format</returns>
        private async Task<ActionResult> SearchBooks_ForceLive(string query)
        {

            if (String.IsNullOrWhiteSpace(query))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "At least one search parameter is required!");

            try
            {
                SearchHistoryModel newSearch = await _searchHistoryService.GetSearchHistoryModelBySearchQuery(query) ?? new SearchHistoryModel { Entity_Id = Guid.NewGuid(), SearchQuery = query };
                newSearch.ModifiedDateTime = DateTime.UtcNow;

                List<VolumeInfo> books = new List<VolumeInfo>();

                var items = await FetchBooksFromAPI(query);

                if (items != null)
                {
                    foreach (Item item in items)
                    {
                        if (item.VolumeInfo.Authors != null)
                            item.VolumeInfo.AuthorList = String.Join(", ", item.VolumeInfo.Authors);
                        if (item.VolumeInfo.Categories != null)
                            item.VolumeInfo.CategoryList = String.Join(", ", item.VolumeInfo.Categories);

                        item.VolumeInfo.Item_id = item.Id;
                        item.VolumeInfo.SearchGuid = newSearch.Entity_Id;
                        newSearch.VoloumeInfo.Add(item.VolumeInfo);


                        books.Add(item.VolumeInfo);
                    }

                    await _searchHistoryService.AddOrUpdateSearchHistories(newSearch);

                    foreach (var item in newSearch.VoloumeInfo)
                    {
                        await _searchHistoryService.AddOrUpdateSearchQueryResults(new SearchQueryResultModel()
                        {
                            SearchId = newSearch.Entity_Id,
                            VolumeInfoId = item.Entity_Id
                        });
                    }
                }

                return Json(books, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// This function will be called when SearchTypes is FiveMinForced
        /// It will send a new HTTP request to Google Library API if there is no previous request with the same query
        /// whitin last 5 mins, the result will be stored (AddOrUpdate) and returned to client
        /// </summary>
        /// <param name="query">Concatenated user search query as string</param>
        /// <returns>List of books in Json format</returns>
        private async Task<ActionResult> SearchBooksWithinNmin(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "At least one search parameter is required!");


            List<VolumeInfo> books;
            try
            {
                SearchHistoryModel newSearch = await _searchHistoryService.GetSearchHistoryModelBySearchQueryWithinNmin(query, MIN_DIFF_TO_UPDATE);
                if (newSearch != null)
                {
                    books = await _bookService.GetBooks_ef_google_search_method(newSearch.Entity_Id);
                }
                else
                {
                    bool newVolumeAdded = false;

                    newSearch = await _searchHistoryService.GetSearchHistoryModelBySearchQuery(query) ?? new SearchHistoryModel { Entity_Id = Guid.NewGuid(), SearchQuery = query };
                    newSearch.ModifiedDateTime = DateTime.UtcNow;
                    books = new List<VolumeInfo>();

                    var items = await FetchBooksFromAPI(query);

                    if (items != null)
                    {
                        foreach (Item item in items)
                        {
                            var volumeFound = await _bookService.GetVolumeByItemId(item.Id);
                            if (volumeFound == null)
                            {
                                if (item.VolumeInfo.Authors != null)
                                    item.VolumeInfo.AuthorList = String.Join(", ", item.VolumeInfo.Authors);
                                if (item.VolumeInfo.Categories != null)
                                    item.VolumeInfo.CategoryList = String.Join(", ", item.VolumeInfo.Categories);

                                item.VolumeInfo.Item_id = item.Id;
                                item.VolumeInfo.SearchGuid = newSearch.Entity_Id;

                                newSearch.VoloumeInfo.Add(item.VolumeInfo);
                                newVolumeAdded = true;

                                books.Add(item.VolumeInfo);
                            }
                            else
                            {
                                books.Add(volumeFound);
                            }

                        }

                        await _searchHistoryService.AddOrUpdateSearchHistories(newSearch);
                        if (newVolumeAdded)
                        {

                            foreach (var item in newSearch.VoloumeInfo)
                            {
                                await _searchHistoryService.AddSearchQueryResults(new SearchQueryResultModel()
                                {
                                    SearchId = newSearch.Entity_Id,
                                    VolumeInfoId = item.Entity_Id
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(books, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method will wall loadbook function of BookReader class with the given query
        /// to make HTTP GET request to Google Library API
        /// </summary>
        /// <param name="query">Concatenated user search query as string</param>
        /// <returns>IEnumerable of books in Json format</returns>
        private async Task<IEnumerable<Item>> FetchBooksFromAPI(string query)
        {
            BookReader bookPr = new BookReader();
            return await bookPr.LoadBook(query);
        } 
        #endregion
    }
}