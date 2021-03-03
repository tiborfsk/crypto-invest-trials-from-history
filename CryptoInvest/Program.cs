using System;

namespace CryptoInvest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inputPath = args.Length < 1 ? "input.json" : args[0];
            var input = InputParser.Parse(inputPath);

            var coinStatesHistoryGenerator = new CoinStatesHistoryGenerator(new HistoricalLinksParser(), new CoinsStatusParser(), input.SleepInSeconds);
            var priceBoard = new PriceBoard();
            var wallet = new Wallet(priceBoard);
            var strategyOperations = new StrategyOperations(wallet, priceBoard, input.TopCoinsCount, input.ReferenceTotalMarketCap.ToReferenceTotalMarketCap());
            var strategy = input.EnableRebalancing
                ? new Strategy(strategyOperations, input.BuyingInterval.ToTimeSpan(), input.RebalancingInterval.ToTimeSpan())
                : new Strategy(strategyOperations, input.BuyingInterval.ToTimeSpan());
            var simulation = new Simulation(priceBoard, strategy);

            simulation.Run(coinStatesHistoryGenerator.GetCoinsStatesHistory(input.From, input.To));

            Console.WriteLine($"Initial market cap: {simulation.InitialMarketCap:0}");
            Console.WriteLine($"Final market cap: {simulation.FinalMarketCap:0}");
            Console.WriteLine($"Market cap change: {(simulation.FinalMarketCap / simulation.InitialMarketCap - 1.0M) * 100.0M:0.00}");
            Console.WriteLine($"Invested: {strategy.Invested:0.0000}");
            Console.WriteLine($"Final value: {wallet.Value:0.0000}");
            Console.WriteLine($"Investing result: {(wallet.Value / strategy.Invested - 1.0M) * 100.0M:0.00}");
            Console.WriteLine($"Wallet:");
            foreach (var walletLine in wallet.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                Console.WriteLine(walletLine);
            }
        }
    }
}
