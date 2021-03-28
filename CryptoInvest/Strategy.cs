using System;

namespace CryptoInvest
{
    public class Strategy
    {
        private readonly decimal investAmount;
        private readonly StrategyBuyOperations strategyBuyOperations;
        private readonly StrategyRebalanceOperations strategyRebalanceOperations;
        private readonly TimeSpan buyingInterval;
        private readonly bool performRebalancing;
        private readonly TimeSpan rebalancingInterval;
        private DateTime? nextBuy;
        private DateTime? nextRebalancing;

        public decimal Invested { get; private set; } = 0;

        public Strategy(decimal investAmount, StrategyBuyOperations strategyBuyOperations, TimeSpan buyingInterval)
        {
            this.investAmount = investAmount;
            this.strategyBuyOperations = strategyBuyOperations;
            this.buyingInterval = buyingInterval;
        }

        public Strategy(decimal investAmount, StrategyBuyOperations strategyBuyOperations, TimeSpan buyingInterval, 
            StrategyRebalanceOperations strategyRebalanceOperations, TimeSpan rebalancingInterval)
            : this(investAmount, strategyBuyOperations, buyingInterval)
        {
            this.strategyRebalanceOperations = strategyRebalanceOperations;
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
                strategyRebalanceOperations.PerformBuyAndRebalancing(investAmount * buys);
                Invested += investAmount * buys;
            }
            else if (buys > 0)
            {
                strategyBuyOperations.PerformBuy(investAmount * buys);
                Invested += investAmount * buys;
            }
            else if (rebalance)
            {
                strategyRebalanceOperations.PerformOnlyRebalancing();
            }
        }
    }
}
