using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class StrategyOperations
    {
        private readonly Wallet wallet;
        private readonly PriceBoard priceBoard;
        private readonly int topCoinsToBuyCount;
        private readonly ReferenceTotalMarketCap referenceTotalMarketCap;

        public StrategyOperations(Wallet wallet, PriceBoard priceBoard, int topCoinsToBuyCount, ReferenceTotalMarketCap referenceTotalMarketCap)
        {
            this.wallet = wallet;
            this.priceBoard = priceBoard;
            this.topCoinsToBuyCount = topCoinsToBuyCount;
            this.referenceTotalMarketCap = referenceTotalMarketCap;
        }

        public virtual void PerformOnlyBuy()
        {
            var totalCashToInvest = 1.0M + SellCoinsNotAlreadyInTop();
            var topCoins = priceBoard.GetTopCoins(topCoinsToBuyCount);

            foreach (var coin in topCoins)
            {
                var singleCoinWallet = wallet.GetOrAddSingleCoinWallet(coin.CoinId);

                singleCoinWallet.BuyByCash(ComputeCashToInvestToCoin(coin.CoinId, totalCashToInvest));
            }
        }

        public virtual void PerformOnlyRebalancing()
        {
            PerformRebalancing(newCashToInvest: 0.0M);
        }

        public virtual void PerformBuyAndRebalancing()
        {
            PerformRebalancing(newCashToInvest: 1.0M);
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
                if (idealBalance > currentBalance)
                {
                    var coinWallet = wallet.GetOrAddSingleCoinWallet(coinId);
                    coinWallet.BuyByCash((idealBalance - currentBalance) * targetWalletValue);
                }
                else if (currentBalance > idealBalance)
                {
                    var coinWallet = wallet.GetOrAddSingleCoinWallet(coinId);
                    coinWallet.SellByCash((currentBalance - idealBalance) * targetWalletValue);
                }
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
