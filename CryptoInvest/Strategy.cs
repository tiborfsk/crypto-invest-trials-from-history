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
        private DateTime? nextBuy;
        private DateTime? nextRebalancing;

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
            var buys = 0;
            while (!nextBuy.HasValue || currentTime >= nextBuy)
            {
                nextBuy = nextBuy.HasValue ? nextBuy + buyingInterval : currentTime + buyingInterval;
                buys++;
            }

            var rebalance = false;
            if (performRebalancing)
            {
                while (!nextRebalancing.HasValue || currentTime >= nextRebalancing)
                {
                    nextRebalancing = nextRebalancing.HasValue ? nextRebalancing + rebalancingInterval : currentTime + rebalancingInterval;
                    rebalance = true;
                }
            }

            if (buys > 0 && rebalance)
            {
                strategyOperations.PerformBuyAndRebalancing(investAmount * buys);
                Invested += investAmount * buys;
            }
            else if (buys > 0)
            {
                strategyOperations.PerformOnlyBuy(investAmount * buys);
                Invested += investAmount * buys;
            }
            else if (rebalance)
            {
                strategyOperations.PerformOnlyRebalancing();
            }
        }
    }
}
