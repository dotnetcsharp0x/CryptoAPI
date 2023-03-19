#nullable disable
//using System.Data.Entity;

using CryptoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoAPI.Data
{
    public class CryptoAPiContext : DbContext
    {
        private string connectionString;
        public CryptoAPiContext ()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("CryptoAPiContext").ToString();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Crypto_Symbols> Crypto_Symbols { get; set; }
        public DbSet<Crypto_Price> Crypto_Price { get; set; }

        protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(20, 10);
        }
        ~CryptoAPiContext()
        {
            //Console.WriteLine($"CryptoAPiContext has deleted");
        }
    }
}
