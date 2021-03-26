using System;
using System.IO;

namespace CryptoInvest
{
    public class FileCache
    {
        private readonly string path;

        public FileCache(string path)
        {
            this.path = path;
        }

        public string Get(string key)
        {
            var fullPath = Path.Combine(path, key);

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(fullPath))
                {
                    return File.ReadAllText(fullPath);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: unable to fetch data from file cache on path {fullPath} ({ex.Message}).");
                return null;
            }
        }

        public void Set(string key, string data)
        {
            var fullPath = Path.Combine(path, key);

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.WriteAllText(fullPath, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: unable to store data to file cache on path {fullPath} ({ex.Message}).");
            }
        }
    }
}
