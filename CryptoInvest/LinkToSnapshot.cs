using System;

namespace CryptoInvest
{
    public record LinkToSnapshot
    {
        public DateTime Time { get; init; }
        public string Link { get; init; }
    }
}
