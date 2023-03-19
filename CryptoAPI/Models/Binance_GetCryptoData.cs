using System;

namespace CryptoAPI.Models
{
    public class Binance_GetCryptoData
    {
        public string timezone { get; set; }
        public long serverTime { get; set; }
        public Binance_rateLimits[] rateLimits { get; set; }
        public Binance_symbols[] symbols { get; set; }
    }
}
