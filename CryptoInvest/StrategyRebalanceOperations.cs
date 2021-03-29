using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class StrategyRebalanceOperations
    {
        private readonly Wallet wallet;
        private readonly PriceBoard priceBoard;
        private readonly InvestBalanceComputation investBalanceComputation;
        private readonly int topCoinsToBuyCount;

        public StrategyRebalanceOperations(Wallet wallet, PriceBoard priceBoard, InvestBalanceComputation investBalanceComputation, 
            int topCoinsToBuyCount)
        {
            this.wallet = wallet;
            this.priceBoard = priceBoard;
            this.investBalanceComputation = investBalanceComputation;
            this.topCoinsToBuyCount = topCoinsToBuyCount;
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
            var topCoins = priceBoard.GetTopCoins(topCoinsToBuyCount);

            foreach (var topCoin in topCoins)
            {
                idealBalances.Add(topCoin.CoinId, investBalanceComputation.ComputeBalanceToInvestToCoin(topCoin.CoinId, topCoins));
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
    }
}
