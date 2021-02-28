using CryptoInvest;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class StrategyOperationsTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TestFirstBuyWithTopCoinsReferenceTotalMarketCap(bool withRebalancing)
        {
            // prepare
            var priceBoard = new PriceBoard();
            priceBoard.PutData(new List<CoinStatus>
            {
                new CoinStatus
                {
                    CoinId = "AAA",
                    MarketCap = 100,
                    Price = 2
                },
                new CoinStatus
                {
                    CoinId = "BBB",
                    MarketCap = 50,
                    Price = 3
                },
                new CoinStatus
                {
                    CoinId = "SHTC",
                    MarketCap = 10,
                    Price = 0.1M
                }
            });
            var wallet = new Wallet(priceBoard);

            var strategyOperations = new StrategyOperations(wallet, priceBoard, topCoinsToBuyCount: 2, ReferenceTotalMarketCap.TopCoins);

            // test
            if (withRebalancing)
            {
                strategyOperations.PerformBuyAndRebalancing();
            }
            else
            {
                strategyOperations.PerformOnlyBuy();
            }

            // assert
            Assert.Equal(1.0M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(0.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(0.666666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(0.111111111111111M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(0.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TestFirstBuyWithAllCoinsReferenceTotalMarketCap(bool withRebalancing)
        {
            // prepare
            var priceBoard = new PriceBoard();
            priceBoard.PutData(new List<CoinStatus>
            {
                new CoinStatus
                {
                    CoinId = "AAA",
                    MarketCap = 100,
                    Price = 2
                },
                new CoinStatus
                {
                    CoinId = "BBB",
                    MarketCap = 50,
                    Price = 3
                },
                new CoinStatus
                {
                    CoinId = "CCC",
                    MarketCap = 30,
                    Price = 1
                },
                new CoinStatus
                {
                    CoinId = "DDD",
                    MarketCap = 10,
                    Price = 1
                }
            });
            var wallet = new Wallet(priceBoard);

            var strategyOperations = new StrategyOperations(wallet, priceBoard, topCoinsToBuyCount: 2, ReferenceTotalMarketCap.AllCoins);

            // test
            if (withRebalancing)
            {
                strategyOperations.PerformBuyAndRebalancing();
            }
            else
            {
                strategyOperations.PerformOnlyBuy();
            }

            // assert
            Assert.Equal(1.0M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(0.3157894736842105263M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(0.6315789473684210526M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(0.1228070175438596491M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(0.3684210526315789473M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }

        [Fact]
        public void TestNonFirstBuyWithTopCoinsReferenceTotalMarketCap()
        {
            // prepare
            var priceBoard = new PriceBoard();
            priceBoard.PutData(new List<CoinStatus>
            {
                new CoinStatus
                {
                    CoinId = "AAA",
                    MarketCap = 100,
                    Price = 2
                },
                new CoinStatus
                {
                    CoinId = "BBB",
                    MarketCap = 50,
                    Price = 3
                }
            });
            var wallet = new Wallet(priceBoard);
            wallet.AddSingleCoinWallet("AAA");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("BBB");
            wallet.GetSingleCoinWallet("BBB").BuyUnits(6);

            var strategyOperations = new StrategyOperations(wallet, priceBoard, topCoinsToBuyCount: 2, ReferenceTotalMarketCap.TopCoins);

            // test
            strategyOperations.PerformOnlyBuy();

            // assert
            Assert.Equal(29M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(5.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(10.66666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(6.111111111111111M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(18.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }

        [Fact]
        public void TestNonFirstRebalanceWithTopCoinsReferenceTotalMarketCap()
        {
            // prepare
            var priceBoard = new PriceBoard();
            priceBoard.PutData(new List<CoinStatus>
            {
                new CoinStatus
                {
                    CoinId = "AAA",
                    MarketCap = 100,
                    Price = 2
                },
                new CoinStatus
                {
                    CoinId = "BBB",
                    MarketCap = 50,
                    Price = 3
                },
                new CoinStatus
                {
                    CoinId = "SHTC",
                    MarketCap = 10,
                    Price = 0.1M
                }
            });
            var wallet = new Wallet(priceBoard);
            wallet.AddSingleCoinWallet("AAA");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("BBB");
            wallet.GetSingleCoinWallet("BBB").BuyUnits(6);

            var strategyOperations = new StrategyOperations(wallet, priceBoard, topCoinsToBuyCount: 2, ReferenceTotalMarketCap.TopCoins);

            // test
            strategyOperations.PerformOnlyRebalancing();

            // assert
            Assert.Equal(28M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(9.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(18.66666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(3.111111111111111M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(9.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }
    }
}
