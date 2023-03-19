using System.ComponentModel.DataAnnotations;

namespace CryptoAPI.Models
{
    public class Crypto_Price : IDisposable
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }

        public void Dispose()
        {
            //Console.WriteLine($"{Symbol} has been disposed");
            //throw new NotImplementedException();
        }
        //~Crypto_Price()
        //{
        //    //Console.WriteLine($"Crypto_Price has deleted");
        //}
    }
}
