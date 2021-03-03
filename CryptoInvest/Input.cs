using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoInvest
{
    public record Input
    {
        [Required]
        public DateTime From { get; init; }
        [Required]
        public DateTime To { get; init; }
        [Required]
        public int SleepInSeconds { get; init; }
        [Required]
        public string BuyingInterval { get; init; }
        [Required]
        public bool EnableRebalancing { get; init; }
        [Required]
        public string RebalancingInterval { get; init; }
        [Required]
        public int TopCoinsCount { get; init; }
        [Required]
        public string ReferenceTotalMarketCap { get; init; }
    }
}
