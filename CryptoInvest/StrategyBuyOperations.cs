using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class StrategyBuyOperations
    {
        private readonly Wallet wallet;
        private readonly PriceBoard priceBoard;
        private readonly int topCoinsToBuyCount;
        private readonly NotTopCoinsDistribution notTopCoinsDistribution;
        private readonly ReferenceTotalMarketCap referenceTotalMarketCap;

        public StrategyBuyOperations(Wallet wallet, PriceBoard priceBoard, int topCoinsToBuyCount,
            NotTopCoinsDistribution notTopCoinsDistribution, ReferenceTotalMarketCap referenceTotalMarketCap)
        {
            this.wallet = wallet;
            this.priceBoard = priceBoard;
            this.topCoinsToBuyCount = topCoinsToBuyCount;
            this.notTopCoinsDistribution = notTopCoinsDistribution;
            this.referenceTotalMarketCap = referenceTotalMarketCap;
        }

        public virtual void PerformBuy(decimal investAmount)
        {
            var cashForSoldCoins = SellCoinsNotAlreadyInTop();
            var topCoins = priceBoard.GetTopCoins(topCoinsToBuyCount);
            var coinsInTopWithoutInvestment = GetCoinsWithoutInvestment(topCoins);
            DistributeAmongCoins(topCoins,
                investAmount + (
                    notTopCoinsDistribution == NotTopCoinsDistribution.AmongAllTopCoins ? cashForSoldCoins : 0
                )
            );
            DistributeAmongCoins(
                coinsInTopWithoutInvestment,
                notTopCoinsDistribution == NotTopCoinsDistribution.AmongNewTopCoins ? cashForSoldCoins : 0
            );
        }

        private List<CoinStatus> GetCoinsWithoutInvestment(List<CoinStatus> coins)
        {
            var coinsWithoutInvestment = new List<CoinStatus>();
            var coinIdsWithInvestment = wallet.SingleCoinWallets.Where(scw => scw.Units > 0).Select(scw => scw.CoinId).ToHashSet();
            foreach (var coin in coins)
            {
                if (!coinIdsWithInvestment.Contains(coin.CoinId))
                {
                    coinsWithoutInvestment.Add(coin);
                }
            }
            return coinsWithoutInvestment;
        }

        private void DistributeAmongCoins(List<CoinStatus> coins, decimal cashToDistribute)
        {
            foreach (var coin in coins)
            {
                var singleCoinWallet = wallet.GetOrAddSingleCoinWallet(coin.CoinId, coin.Name);

                singleCoinWallet.BuyByCash(ComputeCashToInvestToCoin(coin.CoinId, cashToDistribute));
            }
        }
        private decimal SellCoinsNotAlreadyInTop()
        {
            var topCoins = priceBoard.GetTopCoins(topCoinsToBuyCount).Select(tc => tc.CoinId);
            return wallet.SingleCoinWallets.Where(scw => !topCoins.Contains(scw.CoinId)).Sum(scw => scw.SellAll());
        }

        private decimal ComputeBalanceToInvestToCoin(string coinId)
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

        private decimal ComputeCashToInvestToCoin(string coinId, decimal totalCashToInvest) => ComputeBalanceToInvestToCoin(coinId) * totalCashToInvest;
    }
}
