using System;
using System.Linq;

namespace CryptoInvest
{
    public class InvestBalanceComputation
    {
        private readonly PriceBoard priceBoard;
        private readonly int topCoinsToBuyCount;
        private readonly ReferenceTotalMarketCap referenceTotalMarketCap;

        public InvestBalanceComputation(PriceBoard priceBoard, int topCoinsToBuyCount, ReferenceTotalMarketCap referenceTotalMarketCap)
        {
            this.priceBoard = priceBoard;
            this.topCoinsToBuyCount = topCoinsToBuyCount;
            this.referenceTotalMarketCap = referenceTotalMarketCap;
        }

        public decimal ComputeBalanceToInvestToCoin(string coinId)
        {
            var marketCapOfTopCoins = priceBoard.GetTopCoins(topCoinsToBuyCount).Sum(c => c.MarketCap);

            return referenceTotalMarketCap switch
            {
                ReferenceTotalMarketCap.TopCoins => priceBoard.GetMarketCap(coinId) / marketCapOfTopCoins,
                ReferenceTotalMarketCap.AllCoins => (
                    priceBoard.GetMarketCap(coinId) / priceBoard.TotalMarketCap +
                    (1 - marketCapOfTopCoins / priceBoard.TotalMarketCap) / topCoinsToBuyCount
                ),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
