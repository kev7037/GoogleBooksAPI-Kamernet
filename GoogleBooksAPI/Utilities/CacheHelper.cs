using System;
using System.Runtime.Caching;

namespace GoogleBooksAPI.Utilities
{
    public static class CacheHelper
    {
        static MemoryCache cache = new MemoryCache("GoogleLibraryApiCache");

        public static T Get<T>(string key)
        {
            try
            {
                var Result = (T)cache.Get(key);
                return Result;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static void Remove(string key)
        {
            try
            {
                cache.Remove(key);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void Set(string key, object data, DateTime expire)
        {
            try
            {
                cache.Set(key, data, expire);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}