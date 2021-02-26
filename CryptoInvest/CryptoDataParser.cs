using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class CryptoDataParser
    {
        public List<CryptoCoinInTime> GetCryptoCoinsInTime(string link)
        {
            var web = new HtmlWeb();
            var doc = web.Load(link);
            var root = doc.DocumentNode;
            var allCoins = root.SelectNodes("//tr[@class='cmc-table-row']");
            return allCoins
                .Select(row => {
                    return new CryptoCoinInTime
                    {
                        Id = row.ChildNodes[2].FirstChild.InnerText,
                        Name = row.ChildNodes[1].FirstChild.LastChild.InnerText,
                        MarketCap = decimal.Parse(row.ChildNodes[3].FirstChild.InnerText.Trim('$')),
                        Price = decimal.Parse(row.ChildNodes[4].FirstChild.FirstChild.InnerText.Trim('$'))
                    };
                })
                .OrderByDescending(row => row.MarketCap)
                .ToList();
        }
    }
}
