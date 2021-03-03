using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class PriceBoard
    {
        private SortedList<decimal, CoinStatus> sortedCoinsStates;
        private Dictionary<string, CoinStatus> coinsStates;
        private readonly List<string> coinsToIgnore;

        public decimal TotalMarketCap { get; private set; }

        public PriceBoard()
        {
            coinsToIgnore = new List<string>();
        }

        public PriceBoard(List<string> coinsToIgnore)
        {
            this.coinsToIgnore = coinsToIgnore;
        }

        public void PutData(List<CoinStatus> coinsStates)
        {
            TotalMarketCap = coinsStates.Sum(c => c.MarketCap);
            this.coinsStates = coinsStates.ToDictionary(c => c.CoinId, c => c);
            sortedCoinsStates = new SortedList<decimal, CoinStatus>(
                coinsStates.OrderByDescending(cs => cs.MarketCap).ToDictionary(cs => cs.MarketCap, cs => cs),
                new DescendingComparer<decimal>()
            );
        }

        public decimal GetPrice(string id)
        {
            var coinsStates = this.coinsStates ?? throw new InvalidOperationException("No data");
            return coinsStates.TryGetValue(id, out var coinStatus) ? coinStatus.Price : 0.0M;
        }

        public decimal GetMarketCap(string id)
        {
            var coinsStates = this.coinsStates ?? throw new InvalidOperationException("No data");
            return coinsStates.TryGetValue(id, out var coinStatus) ? coinStatus.MarketCap : 0.0M;
        }

        public string GetName(string id)
        {
            var coinsStates = this.coinsStates ?? throw new InvalidOperationException("No data");
            return coinsStates.TryGetValue(id, out var coinStatus) ? coinStatus.Name : null;
        }

        public List<CoinStatus> GetTopCoins(int n)
        {
            return sortedCoinsStates.Where(scs => !coinsToIgnore.Contains(scs.Value.CoinId)).Take(n).Select(cs => cs.Value).ToList();
        }
    }
}
