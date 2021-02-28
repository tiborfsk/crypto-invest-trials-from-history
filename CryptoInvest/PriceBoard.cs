using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class PriceBoard
    {
        private SortedList<string, CoinStatus> coinsStatus;

        public decimal TotalMarketCap { get; private set; }

        public void PutData(List<CoinStatus> cryptoCoin)
        {
            TotalMarketCap = cryptoCoin.Sum(c => c.MarketCap);
            coinsStatus = new SortedList<string, CoinStatus>(cryptoCoin.ToDictionary(c => c.CoinId, c => c));
        }

        public decimal GetPrice(string id)
        {
            var coinsStatus = this.coinsStatus ?? throw new InvalidOperationException("No data");
            return coinsStatus.TryGetValue(id, out var coinStatus) ? coinStatus.Price : 0.0M;
        }

        public decimal GetMarketCap(string id)
        {
            var coinsStatus = this.coinsStatus ?? throw new InvalidOperationException("No data");
            return coinsStatus.TryGetValue(id, out var coinStatus) ? coinStatus.MarketCap : 0.0M;
        }

        public List<CoinStatus> GetTopCoins(int n)
        {
            return coinsStatus.Take(n).Select(cs => cs.Value).ToList();
        }
    }
}
