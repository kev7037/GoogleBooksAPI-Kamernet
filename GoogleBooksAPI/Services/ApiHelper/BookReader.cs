using GoogleBooksAPI.Models.GoogleAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace GoogleBooksAPI.Services.ApiHelper
{
    public class BookReader
    {
        private static string API_KEY = @"AIzaSyCB3yjBSFdU1Nd2YFdYz7bAlPDuHR3xPOg"; //  API key 1 - Google Book API Key
        private readonly string gbooks_api_volume_dir = "volumes?q=";
        public async Task<IEnumerable<Item>> LoadBook(string filter)
        {
            string url = String.Concat(gbooks_api_volume_dir, filter.Replace(' ', '%'), "&orderBy=relevance&maxResults=20&key=", API_KEY);

            try
            {
                using (HttpResponseMessage response = await ApiHelperService.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Root root = await response.Content.ReadAsAsync<Root>();
                        return root.Items;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}