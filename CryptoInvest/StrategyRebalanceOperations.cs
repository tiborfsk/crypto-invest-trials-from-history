using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class StrategyRebalanceOperations
    {
        private readonly Wallet wallet;
        private readonly PriceBoard priceBoard;
        private readonly int topCoinsToBuyCount;
        private readonly ReferenceTotalMarketCap referenceTotalMarketCap;

        public StrategyRebalanceOperations(Wallet wallet, PriceBoard priceBoard, int topCoinsToBuyCount,
            NotTopCoinsDistribution notTopCoinsDistribution, ReferenceTotalMarketCap referenceTotalMarketCap)
        {
            this.wallet = wallet;
            this.priceBoard = priceBoard;
            this.topCoinsToBuyCount = topCoinsToBuyCount;
            this.referenceTotalMarketCap = referenceTotalMarketCap;
        }

        public virtual void PerformOnlyRebalancing()
        {
            PerformRebalancing(newCashToInvest: 0.0M);
        }

        public virtual void PerformBuyAndRebalancing(decimal investAmount)
        {
            PerformRebalancing(newCashToInvest: investAmount);
        }

        private void PerformRebalancing(decimal newCashToInvest)
        {
            var idealBalances = new Dictionary<string, decimal>();
            var currentBalances = new Dictionary<string, decimal>();
            var targetWalletValue = wallet.Value + newCashToInvest;

            foreach (var topCoin in priceBoard.GetTopCoins(topCoinsToBuyCount))
            {
                idealBalances.Add(topCoin.CoinId, ComputeBalanceToInvestToCoin(topCoin.CoinId));
            }

            foreach (var coin in wallet.SingleCoinWallets)
            {
                currentBalances.Add(coin.CoinId, wallet.GetSingleCoinWallet(coin.CoinId).Value / targetWalletValue);
            }

            foreach (var coinId in idealBalances.Keys.Union(currentBalances.Keys))
            {
                var idealBalance = idealBalances.TryGetValue(coinId, out var idealBalanceInDict) ? idealBalanceInDict : 0.0M;
                var currentBalance = currentBalances.TryGetValue(coinId, out var currentBalanceInDict) ? currentBalanceInDict : 0.0M;
                var coinWallet = wallet.GetOrAddSingleCoinWallet(coinId, priceBoard.GetName(coinId));
                if (idealBalance > currentBalance)
                {
                    coinWallet.BuyByCash((idealBalance - currentBalance) * targetWalletValue);
                }
                else if (idealBalance == 0.0M)
                {
                    coinWallet.SellAll();
                }
                else if (currentBalance > idealBalance)
                {
                    coinWallet.SellForCash((currentBalance - idealBalance) * targetWalletValue);
                }
            }
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
    }
}
