using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class StrategyBuyOperations
    {
        private readonly Wallet wallet;
        private readonly PriceBoard priceBoard;
        private readonly InvestBalanceComputation investBalanceComputation;
        private readonly int topCoinsToBuyCount;
        private readonly NotTopCoinsDistribution notTopCoinsDistribution;

        public StrategyBuyOperations(Wallet wallet, PriceBoard priceBoard, InvestBalanceComputation investBalanceComputation, int topCoinsToBuyCount,
            NotTopCoinsDistribution notTopCoinsDistribution)
        {
            this.wallet = wallet;
            this.priceBoard = priceBoard;
            this.investBalanceComputation = investBalanceComputation;
            this.topCoinsToBuyCount = topCoinsToBuyCount;
            this.notTopCoinsDistribution = notTopCoinsDistribution;
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

                singleCoinWallet.BuyByCash(ComputeCashToInvestToCoin(coin.CoinId, coins, cashToDistribute));
            }
        }
        private decimal SellCoinsNotAlreadyInTop()
        {
            var topCoins = priceBoard.GetTopCoins(topCoinsToBuyCount).Select(tc => tc.CoinId);
            return wallet.SingleCoinWallets.Where(scw => !topCoins.Contains(scw.CoinId)).Sum(scw => scw.SellAll());
        }

        private decimal ComputeCashToInvestToCoin(string coinId, List<CoinStatus> coinsGroup, decimal totalCashToInvest) => 
            investBalanceComputation.ComputeBalanceToInvestToCoin(coinId, coinsGroup) * totalCashToInvest;
    }
}
