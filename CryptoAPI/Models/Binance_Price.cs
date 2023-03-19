namespace CryptoAPI.Models
{
    public class Binance_Price
    {
        public string symbol { get; set; }
        public decimal price { get; set; }
        ~Binance_Price()
        {
            //Console.WriteLine($"Binance_Price has deleted");
        }
    }
}
