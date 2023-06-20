using api.allinoneapi.Models;
using api.allinoneapi.Models.Stocks.Polygon;
using api.allinoneapi.Models.Stocks.Tinkoff;
using CryptoAPI.Data;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Tinkoff
{
    public class TinkoffData : IDisposable
    {
        #region UpdateStocks
        public void UpdateStocks()
        {
            try
            {
                var url = "https://hungryapi.ru/api/Stock/GetInstruments/Stocks";
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                var r = client.ExecuteAsync(request).Result.Content;

                var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
                JavaScriptSerializer? js = new JavaScriptSerializer();
                var poly_tickers = js.Deserialize<List<TinkoffMain>>(r);
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    var tickers = (from i in _context.StockInstruments select i).AsNoTracking();
                    foreach (var tick in poly_tickers)
                    {
                        var find_ticker_in_database = (from i in tickers where i.ticker == tick.ticker && i.composite_figi == tick.figi select i).FirstOrDefault();
                        if (find_ticker_in_database == null)
                        {
                            StockInstruments stock = new StockInstruments();
                            stock.market = "stocks";
                            stock.name = tick.name;
                            stock.ticker = tick.ticker;
                            stock.currency_name = tick.currency;
                            stock.active = true;
                            stock.type = "CS";
                            stock.composite_figi = tick.figi;
                            stock.share_class_figi = tick.isin;
                            stock.locale = tick.countryOfRisk;
                            stock.update_date = DateTime.Now;
                            _context.Add(stock);
                            Console.WriteLine("Добавлен тикер:" + tick.ticker + " , name: " + tick.name);
                        }
                    }
                    _context.SaveChanges();
                }
                Content.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            finally { }

        }

        #endregion

        #region UpdateEtfs
        public void UpdateEtfs()
        {
            try
            {
                var url = "https://hungryapi.ru/api/Stock/GetInstruments/Etfs";
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                var r = client.ExecuteAsync(request).Result.Content;

                var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
                JavaScriptSerializer? js = new JavaScriptSerializer();
                var poly_tickers = js.Deserialize<List<TinkoffMain>>(r);
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    var tickers = (from i in _context.StockInstruments select i).AsNoTracking();
                    foreach (var tick in poly_tickers)
                    {
                        var find_ticker_in_database = (from i in tickers where i.ticker == tick.ticker && i.name == tick.name select i).FirstOrDefault();
                        if (find_ticker_in_database == null)
                        {
                            StockInstruments stock = new StockInstruments();
                            stock.market = "stocks";
                            stock.name = tick.name;
                            stock.ticker = tick.ticker;
                            stock.currency_name = tick.currency;
                            stock.active = true;
                            stock.type = "ETF";
                            stock.composite_figi = tick.figi;
                            stock.share_class_figi = tick.isin;
                            stock.locale = tick.countryOfRisk;
                            stock.update_date = DateTime.Now;
                            _context.Add(stock);
                            Console.WriteLine("Добавлен тикер:" + tick.ticker + " , name: " + tick.name);
                        }
                    }
                    _context.SaveChanges();
                }
                Content.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            finally { }

        }

        #endregion

        #region UpdateBonds
        public void UpdateBonds()
        {
            try
            {
                var url = "https://hungryapi.ru/api/Stock/GetInstruments/Bonds";
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                var r = client.ExecuteAsync(request).Result.Content;

                var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
                JavaScriptSerializer? js = new JavaScriptSerializer();
                var poly_tickers = js.Deserialize<List<TinkoffMain>>(r);
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    var tickers = (from i in _context.StockInstruments select i).AsNoTracking();
                    foreach (var tick in poly_tickers)
                    {
                        var find_ticker_in_database = (from i in tickers where i.ticker == tick.classCode && i.name == tick.name select i).FirstOrDefault();
                        if (find_ticker_in_database == null)
                        {
                            StockInstruments stock = new StockInstruments();
                            stock.market = "stocks";
                            stock.name = tick.name;
                            stock.ticker = tick.classCode;
                            stock.currency_name = tick.currency;
                            stock.active = true;
                            stock.type = "WARRANT";
                            stock.composite_figi = tick.figi;
                            stock.share_class_figi = tick.isin;
                            stock.locale = tick.countryOfRisk;
                            stock.update_date = DateTime.Now;
                            _context.Add(stock);
                            Console.WriteLine("Добавлен тикер:" + tick.ticker + " , name: " + tick.name);
                        }
                    }
                    _context.SaveChanges();
                }
                Content.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }
            finally { }
        
        }

        #endregion

        #region DisposeCtor
        public void Dispose()
        {
            
        }
        ~TinkoffData() { }
        #endregion
    }
}
