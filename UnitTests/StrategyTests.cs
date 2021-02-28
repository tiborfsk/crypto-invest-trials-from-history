using CryptoInvest;
using NSubstitute;
using System;
using Xunit;

namespace UnitTests
{
    public class StrategyTests
    {
        [Fact]
        public void PerformOnlyBuys()
        {
            var strategyOperations = Substitute.For<StrategyOperations>(default, default, default, default);
            var strategy = new Strategy(strategyOperations, buyingInterval: TimeSpan.FromDays(2));

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.Received().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 3));

                strategyOperations.Received().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 6));

                strategyOperations.Received().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyOperations.Received().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }
        }

        [Fact]
        public void PerformBuysAndRebalancingInSameTime()
        {
            var strategyOperations = Substitute.For<StrategyOperations>(default, default, default, default);
            var strategy = new Strategy(strategyOperations, buyingInterval: TimeSpan.FromDays(2), rebalancingInterval: TimeSpan.FromDays(2));

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.Received().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 3));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.Received().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 6));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.Received().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }
        }

        [Fact]
        public void PerformBuysAndRebalancingInVariousTimes()
        {
            var strategyOperations = Substitute.For<StrategyOperations>(default, default, default, default);
            var strategy = new Strategy(strategyOperations, buyingInterval: TimeSpan.FromDays(3), rebalancingInterval: TimeSpan.FromDays(4));

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.Received().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyOperations.Received().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 5));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.Received().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 5));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 7));

                strategyOperations.Received().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 13));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.Received().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 14));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyOperations.DidNotReceive().PerformOnlyBuy();
                strategyOperations.Received().PerformBuyAndRebalancing();
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();
            }
        }
    }
}
