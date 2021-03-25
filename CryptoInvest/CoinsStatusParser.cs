using HtmlAgilityPack;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CryptoInvest
{
    public class CoinsStatusParser
    {
        public List<CoinStatus> GetCryptoCoinsInTime(string link)
        {
            var web = new HtmlWeb();
            var doc = web.Load(link);
            var root = doc.DocumentNode;
            var allCoins = root.SelectNodes("//tr[@class='cmc-table-row']");
            return allCoins
                .Select(row => {
                    return new CoinStatus
                    {
                        CoinId = row.ChildNodes[2].FirstChild.InnerText,
                        Name = row.ChildNodes[1].FirstChild.LastChild.InnerText,
                        MarketCap = decimal.Parse(row.ChildNodes[3].FirstChild.InnerText.Trim('$'), CultureInfo.InvariantCulture),
                        Price = decimal.Parse(row.ChildNodes[4].FirstChild.FirstChild.InnerText.Trim('$'), CultureInfo.InvariantCulture)
                    };
                })
                .OrderByDescending(row => row.MarketCap)
                .ToList();
        }
    }
}
