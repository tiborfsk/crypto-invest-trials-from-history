using System;

namespace CryptoInvest
{
    public class Strategy
    {
        private readonly decimal investAmount;
        private readonly StrategyOperations strategyOperations;
        private readonly TimeSpan buyingInterval;
        private readonly bool performRebalancing;
        private readonly TimeSpan rebalancingInterval;
        private DateTime lastBuying;
        private DateTime lastRebalancing;

        public decimal Invested { get; private set; } = 0;

        public Strategy(decimal investAmount, StrategyOperations strategyOperations, TimeSpan buyingInterval)
        {
            this.investAmount = investAmount;
            this.strategyOperations = strategyOperations;
            this.buyingInterval = buyingInterval;
        }

        public Strategy(decimal investAmount, StrategyOperations strategyOperations, TimeSpan buyingInterval, TimeSpan rebalancingInterval)
            : this(investAmount, strategyOperations, buyingInterval)
        {
            this.rebalancingInterval = rebalancingInterval;
            performRebalancing = true;
        }

        public void PerformAction(DateTime currentTime)
        {
            var performBuy = lastBuying + buyingInterval <= currentTime;
            var performRebalance = performRebalancing && lastRebalancing + rebalancingInterval <= currentTime;
            if (performBuy && performRebalance)
            {
                strategyOperations.PerformBuyAndRebalancing(investAmount);
            }
            else if (performBuy)
            {
                strategyOperations.PerformOnlyBuy(investAmount);
            }
            else if (performRebalance)
            {
                strategyOperations.PerformOnlyRebalancing();
            }
            if (performBuy)
            {
                Invested += investAmount;
                lastBuying = currentTime;
            }
            if (performRebalance)
            {
                lastRebalancing = currentTime;
            }
        }
    }
}
