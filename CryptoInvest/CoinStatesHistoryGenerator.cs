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

        public CoinStatesHistoryGenerator(HistoricalLinksParser historicalLinksParser, CoinsStatusParser coinsStatusParser, int sleep)
        {
            this.historicalLinksParser = historicalLinksParser;
            this.coinsStatusParser = coinsStatusParser;
            this.sleep = sleep;
        }

        public SortedList<DateTime, List<CoinStatus>> GetCoinsStatesHistory(DateTime from, DateTime to)
        {
            var historicalLinks = historicalLinksParser.GetHistoricalLinks(from, to);
            var cryptoStates = new Dictionary<DateTime, List<CoinStatus>>();
            historicalLinks.ForEach(hl =>
            {
                Thread.Sleep(sleep * 1000);
                cryptoStates.Add(hl.Time, coinsStatusParser.GetCryptoCoinsInTime(hl.Link));
            });
            return new SortedList<DateTime, List<CoinStatus>>(cryptoStates);
        }
    }
}
