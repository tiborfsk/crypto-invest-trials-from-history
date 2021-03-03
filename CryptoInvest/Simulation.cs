using System;
using System.Collections.Generic;

namespace CryptoInvest
{
    public class Simulation
    {
        private readonly PriceBoard priceBoard;
        private readonly Strategy strategy;

        public decimal InitialMarketCap { get; private set; }

        public decimal FinalMarketCap { get; private set; }

        public Simulation(PriceBoard priceBoard, Strategy strategy)
        {
            this.priceBoard = priceBoard;
            this.strategy = strategy;
        }

        public void Run(SortedList<DateTime, List<CoinStatus>> coinStatesHistory)
        {
            foreach (var coinStates in coinStatesHistory)
            {
                priceBoard.PutData(coinStates.Value);
                InitialMarketCap = InitialMarketCap == default ? priceBoard.TotalMarketCap : InitialMarketCap;
                FinalMarketCap = priceBoard.TotalMarketCap;

                strategy.PerformAction(coinStates.Key);
            }
        }
    }
}
