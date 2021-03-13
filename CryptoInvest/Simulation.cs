using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class Simulation
    {
        private readonly PriceBoard priceBoard;
        private readonly Strategy strategy;
        private readonly CoinUniqueIds coinUniqueIds;

        public decimal InitialMarketCap { get; private set; }

        public decimal FinalMarketCap { get; private set; }

        public Simulation(PriceBoard priceBoard, Strategy strategy, CoinUniqueIds coinUniqueIds)
        {
            this.priceBoard = priceBoard;
            this.strategy = strategy;
            this.coinUniqueIds = coinUniqueIds;
        }

        public void Run(SortedList<DateTime, List<CoinStatus>> coinStatesHistory)
        {
            foreach (var coinStates in coinStatesHistory)
            {
                var coinStatesWithUniqueIds = coinStates.Value
                    .Select(cs => new CoinStatus
                    {
                        CoinId = coinUniqueIds.GetUniqueId(cs.CoinId, cs.Name),
                        MarketCap = cs.MarketCap,
                        Name = cs.Name,
                        Price = cs.Price
                    })
                    .ToList();

                priceBoard.PutData(coinStatesWithUniqueIds);
                InitialMarketCap = InitialMarketCap == default ? priceBoard.TotalMarketCap : InitialMarketCap;
                FinalMarketCap = priceBoard.TotalMarketCap;

                strategy.PerformAction(coinStates.Key);
            }
        }
    }
}
