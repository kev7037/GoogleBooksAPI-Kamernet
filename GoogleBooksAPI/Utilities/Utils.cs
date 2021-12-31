using System;

namespace GoogleBooksAPI.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// This function concatenates newly sent param with search qeury
        /// If searchQuery IsNullOrWhiteSpace, the given param is the first param in search query
        /// </summary>
        /// <param name="searchQuery">user search query</param>
        /// <param name="param">name of new param</param>
        /// <param name="value">value of new param</param>
        /// <returns>concatenated search query (string)</returns>
        public static string addParamToSearchQuery(string searchQuery, string param, string value)
        {
            if (String.IsNullOrWhiteSpace(searchQuery))
                searchQuery = $"{param}={value}";
            else
                searchQuery += $"&{param}={value}";

            return searchQuery;
        }
    }
}