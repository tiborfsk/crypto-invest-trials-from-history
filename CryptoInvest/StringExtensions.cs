using System;
using System.Globalization;

namespace CryptoInvest
{
    public static class StringExtensions
    {
        public static TimeSpan ToTimeSpan(this string @string) => TimeSpan.Parse(@string, CultureInfo.InvariantCulture);

        public static ReferenceTotalMarketCap ToReferenceTotalMarketCap(this string @string) => 
            (ReferenceTotalMarketCap)Enum.Parse(typeof(ReferenceTotalMarketCap), @string);
    }
}
