using CryptoInvest;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class StrategyRebalanceOperationsTests
    {
        private const decimal investAmount = 1.0M;

        [Fact]
        public void TestFirstBuyWithRebalanceWithTopCoinsReferenceTotalMarketCap()
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

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.TopCoins
            );

            strategyRebalanceOperations.PerformBuyAndRebalancing(investAmount);

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

        [Fact]
        public void TestFirstBuyWithRebalanceWithAllCoinsReferenceTotalMarketCap()
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

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.AllCoins
            );

            // test
            strategyRebalanceOperations.PerformBuyAndRebalancing(investAmount);

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
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("BBB", "bbb");
            wallet.GetSingleCoinWallet("BBB").BuyUnits(6);

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.TopCoins
            );

            // test
            strategyRebalanceOperations.PerformOnlyRebalancing();

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

        [Fact]
        public void TestNonFirstRebalanceWithAllCoinsReferenceTotalMarketCap()
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
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("BBB", "bbb");
            wallet.GetSingleCoinWallet("BBB").BuyUnits(6);

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.AllCoins
            );

            // test
            strategyRebalanceOperations.PerformOnlyRebalancing();

            // assert
            Assert.Equal(28M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(8.8421052631578947368M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(17.684210526315789473M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(3.4385964912280701754M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(10.315789473684210526M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }

        [Fact]
        public void TestNonFirstRebalanceAndBuyWithTopCoinsReferenceTotalMarketCap()
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
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("BBB", "bbb");
            wallet.GetSingleCoinWallet("BBB").BuyUnits(6);

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.TopCoins
            );

            // test
            strategyRebalanceOperations.PerformBuyAndRebalancing(investAmount);

            // assert
            Assert.Equal(29M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(9.666666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(19.33333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(3.222222222222222M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(9.666666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }

        [Fact]
        public void TestNonFirstRebalanceAndBuyWithAllCoinsReferenceTotalMarketCap()
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
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("BBB", "bbb");
            wallet.GetSingleCoinWallet("BBB").BuyUnits(6);

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.AllCoins
            );

            // test
            strategyRebalanceOperations.PerformBuyAndRebalancing(investAmount);

            // assert
            Assert.Equal(29M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(9.1578947368421052631M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(18.315789473684210526M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(3.5614035087719298245M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(10.684210526315789473M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }

        [Fact]
        public void TestNonFirstRebalanceWithChangeInTopCoins()
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
                    MarketCap = 20,
                    Price = 3
                }
            });
            var wallet = new Wallet(priceBoard);
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("CCC", "ccc");
            wallet.GetSingleCoinWallet("CCC").BuyUnits(6);

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.TopCoins
            );

            // test
            strategyRebalanceOperations.PerformOnlyRebalancing();

            // assert
            Assert.Equal(28M, wallet.Value, precision: 10);
            Assert.Equal(3, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(9.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(18.66666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(3.111111111111111M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(9.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Value);
        }

        [Fact]
        public void TestNonFirstRebalanceWithBuyWithChangeInTopCoins()
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
                    MarketCap = 20,
                    Price = 3
                }
            });
            var wallet = new Wallet(priceBoard);
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("CCC", "ccc");
            wallet.GetSingleCoinWallet("CCC").BuyUnits(6);

            var strategyRebalanceOperations = new StrategyRebalanceOperations(
                wallet, priceBoard, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins, ReferenceTotalMarketCap.TopCoins
            );

            // test
            strategyRebalanceOperations.PerformBuyAndRebalancing(investAmount);

            // assert
            Assert.Equal(29M, wallet.Value, precision: 10);
            Assert.Equal(3, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(9.666666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(19.33333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(3.222222222222222M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(9.666666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Value);
        }
    }
}
