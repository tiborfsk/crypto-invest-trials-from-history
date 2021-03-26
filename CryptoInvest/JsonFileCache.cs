using System;
using System.Text.Json;

namespace CryptoInvest
{
    public class JsonFileCache<T>
    {
        private readonly FileCache fileCache;

        public JsonFileCache(FileCache fileCache)
        {
            this.fileCache = fileCache;
        }

        public T Get(string key)
        {
            try
            {
                var cachedText = fileCache.Get($"{key}.json");
                if (cachedText == null)
                {
                    return default;
                }
                else
                {
                    return JsonSerializer.Deserialize<T>(cachedText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: unable to deserialize data fetched from file cache with key {key} ({ex.Message}).");
                return default;
            }
        }

        public void Set(string key, T data)
        {
            try
            {
                fileCache.Set($"{key}.json", JsonSerializer.Serialize(data));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: unable to serialize data for storing to file cache with key {key} ({ex.Message}).");
            }
        }
    }
}
