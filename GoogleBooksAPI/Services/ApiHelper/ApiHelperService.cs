using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GoogleBooksAPI.Services.ApiHelper
{
    public static class ApiHelperService
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
