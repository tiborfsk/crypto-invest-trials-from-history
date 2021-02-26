using CryptoInvest;
using System;
using Xunit;

namespace IntegrationTests
{
    public class HistoricalLinksParserTests
    {
        [Fact]
        public void TestGettingHistoricalLinksInsideInterval()
        {
            var parser = new HistoricalLinksParser();
            var links = parser.GetHistoricalLinks(new DateTime(2014, 4, 1), new DateTime(2014, 4, 23));
            Assert.Equal(3, links.Count);
            Assert.Equal("https://coinmarketcap.com/historical/20140406", links[0].Link);
            Assert.Equal(new DateTime(2014, 04, 06), links[0].Time);
            Assert.Equal("https://coinmarketcap.com/historical/20140413", links[1].Link);
            Assert.Equal(new DateTime(2014, 04, 13), links[1].Time);
            Assert.Equal("https://coinmarketcap.com/historical/20140420", links[2].Link);
            Assert.Equal(new DateTime(2014, 04, 20), links[2].Time);
        }

        [Fact]
        public void TestGettingHistoricalLinksJustWithinInterval()
        {
            var parser = new HistoricalLinksParser();
            var links = parser.GetHistoricalLinks(new DateTime(2014, 5, 4), new DateTime(2014, 5, 18));
            Assert.Equal(3, links.Count);
            Assert.Equal("https://coinmarketcap.com/historical/20140504", links[0].Link);
            Assert.Equal(new DateTime(2014, 05, 04), links[0].Time);
            Assert.Equal("https://coinmarketcap.com/historical/20140511", links[1].Link);
            Assert.Equal(new DateTime(2014, 05, 11), links[1].Time);
            Assert.Equal("https://coinmarketcap.com/historical/20140518", links[2].Link);
            Assert.Equal(new DateTime(2014, 05, 18), links[2].Time);
        }

        [Fact]
        public void TestGettingHistoricalLinksEmptyResult()
        {
            var parser = new HistoricalLinksParser();
            var links = parser.GetHistoricalLinks(new DateTime(2014, 8, 12), new DateTime(2014, 8, 14));
            Assert.Empty(links);
        }

        [Fact]
        public void TestGettingAllHistoricalLinksUntil20210226()
        {
            var start = new DateTime(1990, 1, 1);
            var end = new DateTime(2021, 2, 26);
            var parser = new HistoricalLinksParser();
            var links = parser.GetHistoricalLinks(start, end);
            Assert.Equal(409, links.Count);
            Assert.All(links, link =>
            {
                Assert.StartsWith("https://coinmarketcap.com/historical/2", link.Link);
                Assert.True(start < link.Time && link.Time < end);
            });
        }
    }
}
