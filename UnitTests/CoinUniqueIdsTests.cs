using CryptoInvest;
using Xunit;

namespace UnitTests
{
    public class CoinUniqueIdsTests
    {
        [Fact]
        public void TestCoinUniqueIds()
        {
            var coinIds = new CoinUniqueIds();

            Assert.Equal("BTC", coinIds.GetUniqueId("BTC", "Bitcoin"));
            Assert.Equal("BTC", coinIds.GetUniqueId("BTC", "Bitcoin"));
            Assert.Equal("BTC2", coinIds.GetUniqueId("BTC", "Bitcoin2"));
            Assert.Equal("BTC3", coinIds.GetUniqueId("BTC", "Bitcoin3"));
            Assert.Equal("BTC2", coinIds.GetUniqueId("BTC", "Bitcoin2"));
            Assert.Equal("ETH", coinIds.GetUniqueId("ETH", "Ethereum"));
            Assert.Equal("XYZ", coinIds.GetUniqueId("XYZ", "Bitcoin"));
            Assert.Equal("BTC2", coinIds.GetUniqueId("BTC", "Bitcoin2"));
            Assert.Equal("ETH", coinIds.GetUniqueId("ETH", "Ethereum"));
            Assert.Equal("ETH2", coinIds.GetUniqueId("ETH", "Ethereum2"));
        }
    }
}
