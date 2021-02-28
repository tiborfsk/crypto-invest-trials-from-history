namespace CryptoInvest
{
    public record CoinStatus
    {
        public string CoinId { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public decimal MarketCap { get; init; }
    }
}
