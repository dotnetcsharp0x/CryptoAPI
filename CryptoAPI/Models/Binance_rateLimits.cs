namespace CryptoAPI.Models
{
    public class Binance_rateLimits
    {
        public string rateLimitType { get; set; }
        public string interval { get; set; }
        public int intervalNum { get; set; }
        public int limit { get; set; }
    }
}
