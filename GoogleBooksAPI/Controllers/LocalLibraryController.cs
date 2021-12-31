using GoogleBooksAPI.Services.Implementations;
using System.Collections.Generic;
using GoogleBooksAPI.Models.Dto;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;
using System;
using GoogleBooksAPI.Utilities;
using System.Linq;
using GoogleBooksAPI.Models.GoogleAPI;

namespace GoogleBooksAPI.Controllers
{
    public class LocalLibraryController : Controller
    {
        #region Index Page - Local Library
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Modal - Book Detail by bookid (Guid)
        [HttpGet]
        public async Task<ActionResult> BookDetail(Guid bookid)
        {
            if (bookid == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Guid required!");

            var book = await new BookService().GetBookById(bookid);
            return View("~/Views/LocalLibrary/Modals/Mod_BookDetail.cshtml", book);
        }
        #endregion

        #region Search Method - Dapper with pagination

        /// <summary>
        /// This function will call a method to get list of books stored localy with respect to user search param
        /// </summary>
        /// <param name="model">Search Query Object</param>
        /// <returns>DapperResult<IEnumerable<VolumeInfoDto_dapper>></returns>
        [HttpPost]
        public async Task<ActionResult> q(SearchQueryDto model)
        {
            DapperResult<IEnumerable<VolumeInfoDto_dapper>> books = await new BookService().GetBooks_dapper(model);
            return Json(books, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search Method - Dapper With Cache

        /// <summary>
        /// This function will call a method to get list all the books with 5 min cache
        /// </summary>
        /// <param name="model">Search Query Object</param>
        /// <returns>List<VolumeInfo></returns>
        [HttpPost]
        public async Task<ActionResult> qWithCache(SearchQueryDto model)
        {
            DapperResult<List<VolumeInfo>> result =  await new BookService().FilterAndSortCachedBookSearch(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}