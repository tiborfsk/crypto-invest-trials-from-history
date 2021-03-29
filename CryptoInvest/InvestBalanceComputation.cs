using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class InvestBalanceComputation
    {
        private readonly PriceBoard priceBoard;
        private readonly ReferenceTotalMarketCap referenceTotalMarketCap;

        public InvestBalanceComputation(PriceBoard priceBoard, ReferenceTotalMarketCap referenceTotalMarketCap)
        {
            this.priceBoard = priceBoard;
            this.referenceTotalMarketCap = referenceTotalMarketCap;
        }

        public decimal ComputeBalanceToInvestToCoin(string coinId, List<CoinStatus> coinsGroup)
        {
            var marketCapOfCoinsGroup = coinsGroup.Sum(c => c.MarketCap);

            return referenceTotalMarketCap switch
            {
                ReferenceTotalMarketCap.TopCoins => priceBoard.GetMarketCap(coinId) / marketCapOfCoinsGroup,
                ReferenceTotalMarketCap.AllCoins => (
                    priceBoard.GetMarketCap(coinId) / priceBoard.TotalMarketCap +
                    (1 - marketCapOfCoinsGroup / priceBoard.TotalMarketCap) / coinsGroup.Count
                ),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
