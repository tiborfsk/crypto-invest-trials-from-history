using CryptoInvest;
using Xunit;

namespace IntegrationTests
{
    public class CoinsStatusParserTests
    {
        [Fact]
        public void TestGettingCryptoData()
        {
            var cryptoDataParser = new CoinsStatusParser();
            var cryptoData = cryptoDataParser.GetCryptoCoinsInTime("https://coinmarketcap.com/historical/20160522");
            Assert.Equal(200, cryptoData.Count);
            Assert.Contains(cryptoData, crypto =>
                crypto.CoinId == "BTC" &&
                crypto.Name == "Bitcoin" &&
                crypto.MarketCap == 6_842_086_072M &&
                crypto.Price == 439.32M
            );
            Assert.Contains(cryptoData, crypto =>
                crypto.CoinId == "XLM" &&
                crypto.Name == "Stellar" &&
                crypto.MarketCap == 8_921_705M &&
                crypto.Price == 0.001626M
            );
            Assert.Contains(cryptoData, crypto =>
                crypto.CoinId == "HYP" &&
                crypto.Name == "HyperStake" &&
                crypto.MarketCap == 109_302M &&
                crypto.Price == 0.0002833M
            );
        }
    }
}
