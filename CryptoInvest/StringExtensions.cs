using System;
using System.Globalization;

namespace CryptoInvest
{
    public static class StringExtensions
    {
        public static TimeSpan ToTimeSpan(this string @string) => TimeSpan.Parse(@string, CultureInfo.InvariantCulture);

        public static NotTopCoinsDistribution ToNotTopCoinsDistribution(this string @string) => Enum.Parse<NotTopCoinsDistribution>(@string);

        public static ReferenceTotalMarketCap ToReferenceTotalMarketCap(this string @string) => Enum.Parse<ReferenceTotalMarketCap>(@string);
    }
}
