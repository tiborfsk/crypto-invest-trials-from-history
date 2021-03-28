namespace CryptoInvest
{
    public class StrategyOperationsFactory
    {
        private readonly Wallet wallet;
        private readonly PriceBoard priceBoard;
        private readonly int topCoinsToBuyCount;
        private readonly NotTopCoinsDistribution notTopCoinsDistribution;
        private readonly ReferenceTotalMarketCap referenceTotalMarketCap;

        public StrategyOperationsFactory(Wallet wallet, PriceBoard priceBoard, int topCoinsToBuyCount,
            NotTopCoinsDistribution notTopCoinsDistribution, ReferenceTotalMarketCap referenceTotalMarketCap)
        {
            this.wallet = wallet;
            this.priceBoard = priceBoard;
            this.topCoinsToBuyCount = topCoinsToBuyCount;
            this.notTopCoinsDistribution = notTopCoinsDistribution;
            this.referenceTotalMarketCap = referenceTotalMarketCap;
        }

        public StrategyBuyOperations CreateStrategyBuyOperations() =>
            new StrategyBuyOperations(wallet, priceBoard, topCoinsToBuyCount, notTopCoinsDistribution, referenceTotalMarketCap);

        public StrategyRebalanceOperations CreateStrategyRebalanceOperations() =>
            new StrategyRebalanceOperations(wallet, priceBoard, topCoinsToBuyCount, notTopCoinsDistribution, referenceTotalMarketCap);
    }
}
