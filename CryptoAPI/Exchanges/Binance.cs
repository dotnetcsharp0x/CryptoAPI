using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using CryptoAPI.Data;
using System.Diagnostics;
using RestSharp;
using System.Text.Json;
using CryptoAPI.Migrations;
using api.allinoneapi.Models;

namespace CryptoAPI.Exchanges
{
    public class Binance : IDisposable
    {
        //string website = "https://localhost:7151";
        string website = "https://localhost:443";
        //string website = "http://46.22.247.253";
        //string website = "https://hungryapi.ru";
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
                    string url = website + "/api/Crypto/GetPrices";
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
                Thread.Sleep(3000);
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

        #region Test
        public void Test()
        {
            try
            {
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    Crypto_Symbols cr = new Crypto_Symbols();
                    cr.Symbol = "TEST";
                    cr.circulating_supply = 82797235449.0672;
                    cr.total_supply = 82797235449.0672;
                    cr.max_supply = 0;
                    _context.Add(cr);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        #endregion

        #region UpdatePairs
        public async Task<string> UpdatePairs()
        {
            try
            {
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    Binance_symbols[] CryptoPairs;
                    Binance_symbols[] resp;
                    HashSet<Crypto_Symbols> cs = new HashSet<Crypto_Symbols>();
                    _context.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                    //var CurrentPairsInDatabase = (from i in _context.Crypto_Symbols select i).ToArray();
                    string url = website+"/api/Crypto/GetInstruments?page=1";
                    var client = new RestClient(url);
                    var request = new RestRequest(url, Method.Get);
                    request.AddHeader("Content-Type", "application/json");
                    var r = client.ExecuteAsync(request).Result.Content;
                    var alreadyindatabase = (from s in _context.Crypto_Symbols select new Crypto_Symbols()
                    {
                        Symbol=s.Symbol
                    }).AsNoTracking().ToList();
                    if (r is not null)
                    {
                        CryptoPairs = JsonSerializer.Deserialize<Binance_symbols[]>(r);
                        if (CryptoPairs.Length > 0)
                        {


                            foreach (var i in CryptoPairs)
                            {
                                var find_in_database = (from d in alreadyindatabase

                                                        where d.Symbol.Equals(i.symbol)
                                                        select d).FirstOrDefault();

                                if (find_in_database != null)
                                {
                                    foreach (var p in alreadyindatabase.Where(r => r.Symbol == i.symbol))
                                    {
                                        if (i.circulating_supply < 1000000000)
                                        {
                                            p.circulating_supply = i.circulating_supply;
                                        }
                                        else
                                        {
                                            p.circulating_supply = 0;
                                        }
                                        if (i.max_supply < 1000000000)
                                        {
                                            p.max_supply = i.max_supply;
                                        }
                                        else
                                        {
                                            p.max_supply = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    Crypto_Symbols cs_detailed = new Crypto_Symbols();
                                    cs_detailed.Symbol = i.symbol;
                                    cs_detailed.BaseAsset = i.baseAsset;
                                    cs_detailed.QuoteAsset = i.quoteAsset;
                                    if (i.baseAsset == null)
                                    {
                                        Console.WriteLine("null");
                                    }
                                    if (i.circulating_supply != null)
                                    {
                                        if (i.circulating_supply < 1000000000)
                                        {
                                            cs_detailed.circulating_supply = i.circulating_supply;
                                        }
                                        else
                                        {
                                            cs_detailed.circulating_supply = 0;
                                        }
                                    }
                                    else
                                    {
                                        cs_detailed.circulating_supply = 0;
                                    }
                                    if (i.total_supply != null)
                                    {
                                        if (i.total_supply < 1000000000)
                                        {
                                            cs_detailed.total_supply = i.total_supply;
                                        }
                                        else
                                        {
                                            cs_detailed.total_supply = 0;
                                        }
                                    }
                                    else
                                    {
                                        cs_detailed.total_supply = 0;
                                    }
                                    if (i.max_supply != null)
                                    {
                                        if (cs_detailed.max_supply < 1000000000)
                                        {
                                            cs_detailed.max_supply = i.max_supply;
                                        }
                                        else
                                        {
                                            cs_detailed.max_supply = 0;
                                        }
                                    }
                                    else
                                    {
                                        cs_detailed.max_supply = 0;
                                    }
                                    if (i.domination != null)
                                    {
                                        cs_detailed.domination = i.domination;
                                    }
                                    else
                                    {
                                        cs_detailed.domination = 0;
                                    }
                                    cs.Add(cs_detailed);
                                    //_context.Add(cs_detailed);

                                    Console.WriteLine("New crypto pair has been added!: " + cs_detailed.Symbol);
                                }
                            }
                        }
                    }
                    try
                    {
                        await _context.BulkMergeAsync(cs);
                        await _context.BulkSaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);

            }
            finally
            {

            }
            Console.WriteLine("Пары Binance обновлены!");
            return "1";
        }
        #endregion

        #region GetKandles
        public void GetKandles()
        {
            try
            {
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    _context.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                    var CurrentPairsInDatabase = (from i in _context.Crypto_Symbols where i.source == "Binance" 
                                                  select new Crypto_Symbols { Symbol = i.Symbol}
                                                  ).AsNoTracking().ToArray();
                    int count = 0;
                    int minutes = 30;
                    int lines = minutes;
                    foreach (var pair in CurrentPairsInDatabase)
                    {
                        count++;
                        Console.WriteLine($"count: {count}, symbol: {pair.Symbol}");
                        var CryptoKandlesData = new HashSet<Binance_CryptoKandles>();
                        try
                        {
                            string url = website + "/api/Crypto/GetKandles?symbol=" + pair.Symbol+"&minutes="+minutes+"&lines="+lines+"&interval=5M";
                            var client = new RestClient(url);
                            var request = new RestRequest(url, Method.Get);
                            request.AddHeader("Content-Type", "application/json");
                            var r = client.ExecuteAsync(request).Result.Content;
                            if (r != null)
                            {
                                CryptoKandlesData = JsonSerializer.Deserialize<HashSet<Binance_CryptoKandles>>(r);
                                foreach(var cr in CryptoKandlesData)
                                {
                                    _context.Add(cr);
                                    
                                }
                            }
                            //Thread.Sleep(700);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(1000);
                        }
                        finally
                        {
                            
                        }
                        if(count==100)
                        {
                            _context.SaveChanges();
                            count = 0;
                        }
                    }
                    
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

        #region Dispose
        ~Binance()
        {
            //Console.WriteLine($"Binance distructor");
        }

        public void Dispose()
        {
            try
            {

            }
            finally
            {
                //Console.WriteLine($"Binance has been disposed");
            }
        }
        #endregion
    }
}
