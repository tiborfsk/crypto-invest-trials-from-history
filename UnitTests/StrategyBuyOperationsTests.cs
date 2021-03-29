using CryptoInvest;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class StrategyBuyOperationsTests
    {
        private const decimal investAmount = 1.0M;

        [Fact]
        public void TestFirstBuyWithTopCoinsReferenceTotalMarketCap()
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
            var investBalanceComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.TopCoins);
            var strategyBuyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investBalanceComputation, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins
            );

            // test
            strategyBuyOperations.PerformBuy(investAmount);

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
        public void TestFirstBuyWithAllCoinsReferenceTotalMarketCap()
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
            var investBalanceComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.AllCoins);
            var strategyBuyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investBalanceComputation, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins
            );

            // test
            strategyBuyOperations.PerformBuy(investAmount);

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
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("BBB", "bbb");
            wallet.GetSingleCoinWallet("BBB").BuyUnits(6);
            var investBalanceComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.TopCoins);
            var strategyBuyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investBalanceComputation, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins
            );

            // test
            strategyBuyOperations.PerformBuy(investAmount);

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
        public void TestNonFirstBuyWithAllCoinsReferenceTotalMarketCap()
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

            var investBalanceComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.AllCoins);
            var strategyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investBalanceComputation, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins
            );

            // test
            strategyOperations.PerformBuy(investAmount);

            // assert
            Assert.Equal(29M, wallet.Value, precision: 10);
            Assert.Equal(2, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Equal(5.3157894736842105263157M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(10.631578947368421052631M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(6.1228070175438596491228M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(18.368421052631578947368M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
        }

        [Fact]
        public void Test_NonFirstBuyWithChangeInTopCoins_WithDistributionAmongNewTopCoins_WithTopCoinsReferenceMarketCap()
        {
            // prepare
            var priceBoard = new PriceBoard();
            var coinStates = new List<CoinStatus>
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
                },
                new CoinStatus
                {
                    CoinId = "DDD",
                    MarketCap = 15,
                    Price = 2
                },
                new CoinStatus
                {
                    CoinId = "EEE",
                    MarketCap = 10,
                    Price = 4
                },
                new CoinStatus
                {
                    CoinId = "FFF",
                    MarketCap = 10,
                    Price = 22
                }
            };
            priceBoard.PutData(coinStates);
            var wallet = new Wallet(priceBoard);
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("DDD", "ddd");
            wallet.GetSingleCoinWallet("DDD").BuyUnits(6);
            wallet.AddSingleCoinWallet("EEE", "eee");
            wallet.GetSingleCoinWallet("EEE").BuyUnits(7);
            wallet.AddSingleCoinWallet("FFF", "fff");
            wallet.GetSingleCoinWallet("FFF").BuyUnits(8);
            priceBoard.PutData(coinStates.SkipLast(1).ToList());
            var investAmountComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.TopCoins);
            var strategyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investAmountComputation, topCoinsToBuyCount: 3, NotTopCoinsDistribution.AmongNewTopCoins
            );

            // test
            strategyOperations.PerformBuy(investAmount);

            // assert
            Assert.Equal(51M, wallet.Value, precision: 10);
            Assert.Equal(6, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "CCC");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "DDD");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "EEE");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "FFF");
            Assert.Equal(5.2941176470588235M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(10.588235294117647M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(9.6218487394957983M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(28.865546218487395M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
            Assert.Equal(3.8487394957983193M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Units, precision: 10);
            Assert.Equal(11.546218487394958M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Value, precision: 10);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "DDD").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "DDD").Value);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "EEE").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "EEE").Value);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "FFF").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "FFF").Value);
        }

        [Fact]
        public void Test_NonFirstBuyWithChangeInTopCoins_WithDistributionAmongNewTopCoins_WithAllCoinsReferenceMarketCap()
        {
            // prepare
            var priceBoard = new PriceBoard();
            var coinStates = new List<CoinStatus>
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
                },
                new CoinStatus
                {
                    CoinId = "DDD",
                    MarketCap = 15,
                    Price = 2
                },
                new CoinStatus
                {
                    CoinId = "EEE",
                    MarketCap = 10,
                    Price = 4
                },
                new CoinStatus
                {
                    CoinId = "FFF",
                    MarketCap = 10,
                    Price = 22
                }
            };
            priceBoard.PutData(coinStates);
            var wallet = new Wallet(priceBoard);
            wallet.AddSingleCoinWallet("AAA", "aaa");
            wallet.GetSingleCoinWallet("AAA").BuyUnits(5);
            wallet.AddSingleCoinWallet("DDD", "ddd");
            wallet.GetSingleCoinWallet("DDD").BuyUnits(6);
            wallet.AddSingleCoinWallet("EEE", "eee");
            wallet.GetSingleCoinWallet("EEE").BuyUnits(7);
            wallet.AddSingleCoinWallet("FFF", "fff");
            wallet.GetSingleCoinWallet("FFF").BuyUnits(8);
            priceBoard.PutData(coinStates.SkipLast(1).ToList());
            var investAmountComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.AllCoins);
            var strategyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investAmountComputation, topCoinsToBuyCount: 3, NotTopCoinsDistribution.AmongNewTopCoins
            );

            // test
            strategyOperations.PerformBuy(investAmount);

            // assert
            Assert.Equal(51M, wallet.Value, precision: 10);
            Assert.Equal(6, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "CCC");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "DDD");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "EEE");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "FFF");
            Assert.Equal(5.27777777777777778M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(10.5555555555555556M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(7.79202279202279202M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(23.3760683760683761M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
            Assert.Equal(5.68945868945868946M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Units, precision: 10);
            Assert.Equal(17.0683760683760684M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Value, precision: 10);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "DDD").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "DDD").Value);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "EEE").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "EEE").Value);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "FFF").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "FFF").Value);
        }

        [Fact]
        public void TestNonFirstBuyWithChangeInTopCoinsWithDistributionAmongAllTopCoins()
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
            var investAmountComputation = new InvestBalanceComputation(priceBoard, ReferenceTotalMarketCap.TopCoins);
            var strategyOperations = new StrategyBuyOperations(
                wallet, priceBoard, investAmountComputation, topCoinsToBuyCount: 2, NotTopCoinsDistribution.AmongAllTopCoins
            );

            // test
            strategyOperations.PerformBuy(investAmount);

            // assert
            Assert.Equal(29M, wallet.Value, precision: 10);
            Assert.Equal(3, wallet.SingleCoinWallets.Count);
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "AAA");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "BBB");
            Assert.Contains(wallet.SingleCoinWallets, scw => scw.CoinId == "CCC");
            Assert.Equal(11.333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Units, precision: 10);
            Assert.Equal(22.666666666666666M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "AAA").Value, precision: 10);
            Assert.Equal(2.1111111111111111M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Units, precision: 10);
            Assert.Equal(6.3333333333333333M, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "BBB").Value, precision: 10);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Units);
            Assert.Equal(0, wallet.SingleCoinWallets.Single(scw => scw.CoinId == "CCC").Value);
        }
    }
}
