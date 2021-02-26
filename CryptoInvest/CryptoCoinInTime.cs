namespace CryptoInvest
{
    public record CryptoCoinInTime
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public decimal MarketCap { get; init; }
    }
}
