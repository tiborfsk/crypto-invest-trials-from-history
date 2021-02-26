using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class HistoricalLinksParser
    {
        public List<LinkToSnapshot> GetHistoricalLinks(DateTime from, DateTime to)
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://coinmarketcap.com/historical");
            var root = doc.DocumentNode;
            var allLinks = root.SelectNodes("//a[@class='historical-link cmc-link']");
            return allLinks
                .Select(link =>
                {
                    var hrefValue = link.Attributes.Where(attr => attr.Name == "href").Single().Value;
                    return new LinkToSnapshot
                    {
                        Time = new DateTime(
                            int.Parse(hrefValue.Substring(12, 4)),
                            int.Parse(hrefValue.Substring(16, 2)),
                            int.Parse(hrefValue.Substring(18, 2))
                        ),
                        Link = $"https://coinmarketcap.com/{hrefValue.Trim('/')}"
                    };
                })
                .OrderBy(link => link.Time)
                .Where(link => link.Time >= from && link.Time <= to)
                .ToList();
        }
    }
}
