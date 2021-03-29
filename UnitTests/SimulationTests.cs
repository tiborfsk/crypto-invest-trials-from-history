using CryptoInvest;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public class SimulationTests
    {
        [Fact]
        public void TestCoinsWithSameId()
        {
            var priceBoard = new PriceBoard();
            var wallet = new Wallet(priceBoard);
            var investAmountComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.TopCoins);
            var strategyBuyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investAmountComputation, topCoinsToBuyCount: 3, NotTopCoinsDistribution.AmongAllTopCoins
            );
            var strategy = new Strategy(10, strategyBuyOperations, TimeSpan.FromDays(1));
            var simulation = new Simulation(priceBoard, strategy, new CoinUniqueIds());
            simulation.Run(new SortedList<DateTime, List<CoinStatus>>(new Dictionary<DateTime, List<CoinStatus>>
            {
                [new DateTime(2021, 1, 1)] = new List<CoinStatus>
                {
                    new CoinStatus
                    {
                        CoinId = "BTC",
                        Name = "Bitcoin",
                        MarketCap = 10,
                        Price = 1
                    },
                    new CoinStatus
                    {
                        CoinId = "GLC",
                        Name = "Goldcoin",
                        MarketCap = 2,
                        Price = 0.5M
                    },
                    new CoinStatus
                    {
                        CoinId = "GLC",
                        Name = "Globalcoin",
                        MarketCap = 3,
                        Price = 2
                    }
                }
            }));
            Assert.Equal(3, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BTC" && scw.CoinName == "Bitcoin");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "GLC" && scw.CoinName == "Goldcoin");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "GLC2" && scw.CoinName == "Globalcoin");
        }
    }
}
