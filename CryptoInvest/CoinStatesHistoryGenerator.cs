using Polly;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CryptoInvest
{
    public class CoinStatesHistoryGenerator
    {
        private readonly HistoricalLinksParser historicalLinksParser;
        private readonly CoinsStatusParser coinsStatusParser;
        private readonly int sleep;
        private readonly JsonFileCache<List<CoinStatus>> jsonFileCache;

        public CoinStatesHistoryGenerator(HistoricalLinksParser historicalLinksParser, CoinsStatusParser coinsStatusParser, 
            int sleep, JsonFileCache<List<CoinStatus>> jsonFileCache)
        {
            this.historicalLinksParser = historicalLinksParser;
            this.coinsStatusParser = coinsStatusParser;
            this.sleep = sleep;
            this.jsonFileCache = jsonFileCache;
        }

        public SortedList<DateTime, List<CoinStatus>> GetCoinsStatesHistory(DateTime from, DateTime to)
        {
            var historicalLinks = historicalLinksParser.GetHistoricalLinks(from, to);
            var cryptoStates = new Dictionary<DateTime, List<CoinStatus>>();
            historicalLinks.ForEach(hl =>
            {
                var cacheKey = hl.Time.ToString("yyyy-MM-dd");
                var cachedCryptoStates = jsonFileCache.Get(cacheKey);
                if (cachedCryptoStates != null)
                {
                    cryptoStates.Add(hl.Time, cachedCryptoStates);
                }
                else
                {
                    Thread.Sleep(sleep * 1000);
                    Policy
                        .Handle<Exception>()
                        .WaitAndRetry(new[]
                        {
                            TimeSpan.FromSeconds(30),
                            TimeSpan.FromSeconds(120),
                            TimeSpan.FromSeconds(600),
                            TimeSpan.FromSeconds(1800)
                        })
                        .Execute(() =>
                        {
                            Console.WriteLine($"{hl.Time:yyyy MM dd}...");
                            var coinStates = coinsStatusParser.GetCryptoCoinsInTime(hl.Link);
                            cryptoStates.Add(hl.Time, coinStates);
                            jsonFileCache.Set(cacheKey, coinStates);
                        });
                }
            });
            return new SortedList<DateTime, List<CoinStatus>>(cryptoStates);
        }
    }
}
