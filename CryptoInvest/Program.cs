using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CryptoInvest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inputPath = args.Length < 1 ? "input.json" : args[0];
            var input = InputParser.Parse(inputPath);
            foreach (var property in typeof(Input).GetProperties())
            {
                var value = property.GetValue(input);
                Console.WriteLine($"{property.Name}: {(value is string[] arrayValue ? string.Join(", ", arrayValue) : value)}");
            }

            var coinStatesHistoryGenerator = new CoinStatesHistoryGenerator(
                new HistoricalLinksParser(), 
                new CoinsStatusParser(), 
                input.SleepInSeconds, 
                new JsonFileCache<List<CoinStatus>>(new FileCache(Path.Combine(AppContext.BaseDirectory, "cache")))
            );
            var priceBoard = new PriceBoard(input.CoinsToIgnore.ToList());
            var wallet = new Wallet(priceBoard);
            var strategyOperations = new StrategyOperationsFactory(
                wallet, priceBoard, input.TopCoinsCount,
                input.NotTopCoinsDistribution.ToNotTopCoinsDistribution(), input.ReferenceTotalMarketCap.ToReferenceTotalMarketCap()
            );
            var strategy = input.EnableRebalancing
                ? new Strategy(
                    input.InvestAmount, strategyOperations.CreateStrategyBuyOperations(), input.BuyingInterval.ToTimeSpan(), 
                    strategyOperations.CreateStrategyRebalanceOperations(), input.RebalancingInterval.ToTimeSpan()
                ) : new Strategy(input.InvestAmount, strategyOperations.CreateStrategyBuyOperations(), input.BuyingInterval.ToTimeSpan());
            var simulation = new Simulation(priceBoard, strategy, new CoinUniqueIds());

            simulation.Run(coinStatesHistoryGenerator.GetCoinsStatesHistory(input.From, input.To));

            Console.WriteLine($"Initial market cap: ${simulation.InitialMarketCap:0}");
            Console.WriteLine($"Final market cap: ${simulation.FinalMarketCap:0}");
            Console.WriteLine($"Market cap change: {(simulation.FinalMarketCap / simulation.InitialMarketCap - 1.0M) * 100.0M:0.00}%");
            Console.WriteLine($"Invested: ${strategy.Invested:0.0000}");
            Console.WriteLine($"Final value: ${wallet.Value:0.0000}");
            Console.WriteLine($"Investing result: {(wallet.Value / strategy.Invested - 1.0M) * 100.0M:0.00}%");
            Console.WriteLine($"Wallet:");
            foreach (var walletLine in wallet.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                Console.WriteLine(walletLine);
            }
        }
    }
}
