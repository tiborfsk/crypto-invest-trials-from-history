using System;

namespace CryptoInvest
{
    public class Strategy
    {
        private readonly StrategyOperations strategyOperations;
        private readonly TimeSpan buyingInterval;
        private readonly bool performRebalancing;
        private readonly TimeSpan rebalancingInterval;
        private DateTime lastBuying;
        private DateTime lastRebalancing;

        public Strategy(StrategyOperations strategyOperations, TimeSpan buyingInterval)
        {
            this.strategyOperations = strategyOperations;
            this.buyingInterval = buyingInterval;
        }

        public Strategy(StrategyOperations strategyOperations, TimeSpan buyingInterval, TimeSpan rebalancingInterval)
            : this(strategyOperations, buyingInterval)
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
                strategyOperations.PerformBuyAndRebalancing();
            }
            else if (performBuy)
            {
                strategyOperations.PerformOnlyBuy();
            }
            else if (performRebalance)
            {
                strategyOperations.PerformOnlyRebalancing();
            }
            if (performBuy)
            {
                lastBuying = currentTime;
            }
            if (performRebalance)
            {
                lastRebalancing = currentTime;
            }
        }
    }
}
