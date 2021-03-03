using System;

namespace CryptoInvest
{
    public record Input
    {
        public DateTime From { get; init; }
        public DateTime To { get; init; }
        public int SleepInSeconds { get; init; }
        public string BuyingInterval { get; init; }
        public bool EnableRebalancing { get; init; }
        public string RebalancingInterval { get; init; }
        public decimal InvestAmount { get; init; }
        public int TopCoinsCount { get; init; }
        public string ReferenceTotalMarketCap { get; init; }
        public string[] CoinsToIgnore { get; init; }
    }
}
