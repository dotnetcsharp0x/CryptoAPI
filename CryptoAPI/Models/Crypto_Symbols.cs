namespace CryptoAPI.Models
{
    public class Crypto_Symbols : IDisposable
    {
        public int Id { get; set; }
        public string? Symbol { get; set; }
        public string? BaseAsset { get; set; }
        public string? QuoteAsset { get; set; }
        public void Dispose()
        {
            //Console.WriteLine($"{Symbol} has been disposed");
            //throw new NotImplementedException();
        }
    }
}
