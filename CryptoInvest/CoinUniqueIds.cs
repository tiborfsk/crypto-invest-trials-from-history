using System.Collections.Generic;

namespace CryptoInvest
{
    public class CoinUniqueIds
    {
        private readonly Dictionary<string, string> usedIds = new Dictionary<string, string>();

        public string GetUniqueId(string id, string value)
        {
            int? index = null;
            while (usedIds.TryGetValue($"{id}{index}", out var storedValue) && value != storedValue)
            {
                index = index.HasValue ? index.Value + 1 : 2;
            }
            usedIds[$"{id}{index}"] = value;
            return $"{id}{index}";
        }
    }
}
