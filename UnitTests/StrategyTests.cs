using CryptoInvest;
using NSubstitute;
using System;
using Xunit;

namespace UnitTests
{
    public class StrategyTests
    {
        private const decimal investAmount = 1.0M;

        [Fact]
        public void PerformOnlyBuys()
        {
            var strategyOperations = Substitute.For<StrategyOperations>(default, default, default, default);
            var strategy = new Strategy(investAmount, strategyOperations, buyingInterval: TimeSpan.FromDays(2));

            {
                Assert.Equal(0, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.Received().PerformOnlyBuy(investAmount);
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 3));

                strategyOperations.Received().PerformOnlyBuy(investAmount);
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 6));

                strategyOperations.Received().PerformOnlyBuy(investAmount);
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(3, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 7));

                strategyOperations.Received().PerformOnlyBuy(investAmount);
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(4, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyOperations.Received().PerformOnlyBuy(investAmount * 11);
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(15, strategy.Invested);
            }
        }

        [Fact]
        public void PerformBuysAndRebalancingInSameTime()
        {
            var strategyOperations = Substitute.For<StrategyOperations>(default, default, default, default);
            var strategy = new Strategy(1.0M, strategyOperations, buyingInterval: TimeSpan.FromDays(2), rebalancingInterval: TimeSpan.FromDays(2));

            {
                Assert.Equal(0, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 3));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 6));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(3, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 7));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(4, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount * 11);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(15, strategy.Invested);
            }
        }

        [Fact]
        public void PerformBuysAndRebalancingInVariousTimes()
        {
            var strategyOperations = Substitute.For<StrategyOperations>(default, default, default, default);
            var strategy = new Strategy(investAmount, strategyOperations, buyingInterval: TimeSpan.FromDays(3), rebalancingInterval: TimeSpan.FromDays(4));

            {
                Assert.Equal(0, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyOperations.Received().PerformOnlyBuy(investAmount);
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 5));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.Received().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 5));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 7));

                strategyOperations.Received().PerformOnlyBuy(investAmount);
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(3, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 13));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount * 2);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(5, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 14));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyOperations.ClearReceivedCalls();

                Assert.Equal(5, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyOperations.DidNotReceive().PerformOnlyBuy(Arg.Any<decimal>());
                strategyOperations.Received().PerformBuyAndRebalancing(investAmount * 5);
                strategyOperations.DidNotReceive().PerformOnlyRebalancing();

                Assert.Equal(10, strategy.Invested);
            }
        }
    }
}
