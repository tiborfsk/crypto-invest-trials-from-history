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
        public void PerformBuys()
        {
            var strategyBuyOperations = Substitute.For<StrategyBuyOperations>(default, default, default, default, default);
            var strategy = new Strategy(investAmount, strategyBuyOperations, buyingInterval: TimeSpan.FromDays(2));

            {
                Assert.Equal(0, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyBuyOperations.Received().PerformBuy(investAmount);

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 3));

                strategyBuyOperations.Received().PerformBuy(investAmount);

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 6));

                strategyBuyOperations.Received().PerformBuy(investAmount);

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(3, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 7));

                strategyBuyOperations.Received().PerformBuy(investAmount);

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(4, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyBuyOperations.Received().PerformBuy(investAmount * 11);

                strategyBuyOperations.ClearReceivedCalls();

                Assert.Equal(15, strategy.Invested);
            }
        }

        [Fact]
        public void PerformBuysAndRebalancingInSameTime()
        {
            var strategyBuyOperations = Substitute.For<StrategyBuyOperations>(default, default, default, default, default);
            var strategyRebalanceOperations = Substitute.For<StrategyRebalanceOperations>(default, default, default, default, default);
            var strategy = new Strategy(1.0M, strategyBuyOperations, buyingInterval: TimeSpan.FromDays(2),
                strategyRebalanceOperations, rebalancingInterval: TimeSpan.FromDays(2)
            );

            {
                Assert.Equal(0, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 3));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 6));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(3, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 7));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(4, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount * 11);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(15, strategy.Invested);
            }
        }

        [Fact]
        public void PerformBuysAndRebalancingInVariousTimes()
        {
            var strategyBuyOperations = Substitute.For<StrategyBuyOperations>(default, default, default, default, default);
            var strategyRebalanceOperations = Substitute.For<StrategyRebalanceOperations>(default, default, default, default, default);
            var strategy = new Strategy(investAmount, strategyBuyOperations, buyingInterval: TimeSpan.FromDays(3), 
                strategyRebalanceOperations, rebalancingInterval: TimeSpan.FromDays(4));

            {
                Assert.Equal(0, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 1));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 2));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(1, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 4));

                strategyBuyOperations.Received().PerformBuy(investAmount);
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 5));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 5));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(2, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 7));

                strategyBuyOperations.Received().PerformBuy(investAmount);
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(3, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 13));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount * 2);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(5, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 14));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformBuyAndRebalancing(Arg.Any<decimal>());
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                strategyBuyOperations.ClearReceivedCalls();
                strategyRebalanceOperations.ClearReceivedCalls();

                Assert.Equal(5, strategy.Invested);
            }

            {
                strategy.PerformAction(new DateTime(2021, 1, 30));

                strategyBuyOperations.DidNotReceive().PerformBuy(Arg.Any<decimal>());
                strategyRebalanceOperations.Received().PerformBuyAndRebalancing(investAmount * 5);
                strategyRebalanceOperations.DidNotReceive().PerformOnlyRebalancing();

                Assert.Equal(10, strategy.Invested);
            }
        }
    }
}
