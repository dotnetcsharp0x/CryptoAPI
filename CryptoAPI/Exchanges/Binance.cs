using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using CryptoAPI.Models;
using CryptoAPI.Data;
using System.Diagnostics;
using RestSharp;
using System.Text.Json;

namespace CryptoAPI.Exchanges
{
    public class Binance : IDisposable
    {
        //string website = "https://localhost:7151";
        //string website = "http://46.22.247.253";
        string website = "https://hungryapi.ru";
        public Binance()
        {
        }

        #region UpdatePrices
        public void UpdatePrices()
        {
            Binance_Price[] BinancePrices;
            //List<Crypto_Price> CryptoPricesList = new List<Crypto_Price>();
            try
            {
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    _context.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                    var CurrentPairsInDatabase = (from i in _context.Crypto_Price select i).ToArray();
                    string url = website + "/api/instruments/UpdateCurrentPrice";
                    var client = new RestClient(url);
                    var request = new RestRequest(url, Method.Get);
                    request.AddHeader("Content-Type", "application/json");
                    var r = client.ExecuteAsync(request).Result.Content;
                    
                    BinancePrices = JsonSerializer.Deserialize<Binance_Price[]>(r);
                    if (BinancePrices is not null)
                    {
                        foreach (var a in BinancePrices)
                        {
                            Crypto_Price CryptoPrices = (from d in CurrentPairsInDatabase where d.Symbol == a.symbol select d).FirstOrDefault();
                            if (CryptoPrices is null)
                            {
                                CryptoPrices = new Crypto_Price();
                                CryptoPrices.Symbol = a.symbol;
                                CryptoPrices.Price = a.price;
                                CryptoPrices.DateTime = DateTime.Now;
                                _context.Add(CryptoPrices);
                            }
                            else
                            {
                                CryptoPrices.Symbol = a.symbol;
                                CryptoPrices.Price = a.price;
                                CryptoPrices.DateTime = DateTime.Now;
                            }
                            CryptoPrices.Dispose();
                        }
                        _context.SaveChanges();
                    }
                    
                    CurrentPairsInDatabase = null;
                    url = null;
                    client = null;
                    request = null;
                    r = null;
                    _context.Dispose();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
        }
        #endregion
        #region UpdatePairs
        public void UpdatePairs()
        {
            try
            {
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    Binance_symbols[] CryptoPairs;
                    Binance_symbols[] resp;
                    List<Crypto_Symbols> cs = new List<Crypto_Symbols>();
                    _context.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                    var CurrentPairsInDatabase = (from i in _context.Crypto_Symbols select i).ToArray();
                    string url = website+"/api/instruments/UpdatePairs";
                    var client = new RestClient(url);
                    var request = new RestRequest(url, Method.Get);
                    request.AddHeader("Content-Type", "application/json");
                    var r = client.ExecuteAsync(request).Result.Content;
                    if (r is not null)
                    {
                        CryptoPairs = JsonSerializer.Deserialize<Binance_symbols[]>(r);
                        if (CryptoPairs.Length > 0)
                        {
                            foreach (var i in CryptoPairs)
                            {
                                Crypto_Symbols cs_detailed = new Crypto_Symbols();
                                cs_detailed.Symbol = i.symbol;
                                cs_detailed.BaseAsset = i.baseAsset;
                                cs_detailed.QuoteAsset = i.quoteAsset;
                                cs.Add(cs_detailed);
                                _context.Add(cs_detailed);
                                Console.WriteLine("New crypto pair has been added!: " + cs_detailed.Symbol);
                            }
                        }
                    }
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {

            }
        }
        #endregion
        ~Binance()
        {
            Console.WriteLine($"Binance distructor");
        }

        public void Dispose()
        {
            try
            {

            }
            finally
            {
                Console.WriteLine($"Binance has been disposed");
            }
        }

    }
}
