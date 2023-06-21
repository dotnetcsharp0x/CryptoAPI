using api.allinoneapi.Models;
using api.allinoneapi.Models.Stocks.Polygon;
using api.allinoneapi.Models.Stocks.Polygon.Dividends;
using api.allinoneapi.Models.Stocks.Polygon.News;
using api.allinoneapi.Models.Stocks.Tinkoff;
using CryptoAPI.Data;
using CryptoAPI.Polygon;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CryptoAPI.Stocks.Polygon
{
    public class Poly : IDisposable
    {
        string url_from = "https://localhost:443/api/Stock/GetInstruments";
        private static string api = "";
        //private string website = "https://hungryapi.ru";
        //private string website = "https://localhost:7151";
        private string website = "https://localhost:443";
        public Poly() {
            XmlDocument xApi = new XmlDocument();
            xApi.Load("api.xml");
            XmlElement? xRootApi = xApi.DocumentElement;
            if (xRootApi != null)
            {
                foreach (XmlElement xnode in xRootApi)
                {
                    api = "&apiKey=" + xnode.ChildNodes[1].InnerText;
                }
            }
        }

        #region GetDividendsTinkoff
        public async Task<string> GetDividendsTinkoff()
        {
            var url = website + "/api/Stock/GetInstruments/Stocks";
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            var r = client.ExecuteAsync(request).Result.Content;

            var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
            JavaScriptSerializer? js = new JavaScriptSerializer();
            var poly_tickers = js.Deserialize<List<TinkoffMain>>(r);
            
            
            using (CryptoAPIContext _context = new CryptoAPIContext())
            {
                foreach (var i in poly_tickers)
                {
                    //var current_in_database = (from n in _context.StockDividends where n.ticker == i.ticker select n).AsNoTracking().ToList();
                    
                    var url_dividend = website + "/api/Stock/GetInstruments/GetDividends?figi=" +
                        i.figi +
                        "&dateFrom=" +
                        DateTime.Now.AddYears(-20) +
                        "&dateTo=" +
                        DateTime.Now +
                        "";
                    var client_dividend = new RestClient(url_dividend);
                    var request_dividend = new RestRequest(url_dividend, Method.Get);
                    request_dividend.AddHeader("Content-Type", "application/json");
                    var r_dividend = client_dividend.ExecuteAsync(request_dividend).Result.Content;

                    var Content_dividend = new StringContent(r_dividend.ToString(), Encoding.UTF8, "application/json");
                    JavaScriptSerializer? js_dividend = new JavaScriptSerializer();
                    var poly_tickers_dividend = js_dividend.Deserialize<api.allinoneapi.Models.Tinkoff.Dividends.Dividends>(r_dividend);
                    List<api.allinoneapi.Models.Stocks.Polygon.Dividends.Result> divs = new List<api.allinoneapi.Models.Stocks.Polygon.Dividends.Result>();
                    foreach (var d in poly_tickers_dividend.dividends) {
                        var pay_date = d.paymentDate.ToString("yyy-MM-dd");
                        Console.WriteLine("Смотрю: " + i.figi + ", с датой: " + pay_date);
                        var current_in_database = (from n in _context.StockDividends where n.ticker == i.ticker && n.pay_date == pay_date select n).AsNoTracking().ToList();
                        if(current_in_database.Count() == 0)
                        {
                            api.allinoneapi.Models.Stocks.Polygon.Dividends.Result div = new api.allinoneapi.Models.Stocks.Polygon.Dividends.Result();
                            div.cash_amount = Convert.ToDouble(d.dividendNet.units +"."+ Convert.ToString(d.dividendNet.nano));
                            div.currency = d.dividendNet.currency;
                            div.declaration_date = d.declaredDate.ToString("yyyy-MM-dd");
                            div.dividend_type = d.dividendType;
                            div.ex_dividend_date = d.lastBuyDate.ToString("yyyy-MM-dd");
                            div.frequency = 1;
                            div.pay_date = d.paymentDate.ToString("yyyy-MM-dd");
                            div.record_date = d.recordDate.ToString("yyyy-MM-dd");
                            div.ticker = i.ticker;
                            div.update_date = DateTime.Now;
                            divs.Add(div);
                        }
                    }
                    client_dividend.Dispose();
                    Content_dividend.Dispose();

                    await _context.BulkMergeAsync(divs);
                    await _context.BulkSaveChangesAsync();
                    Thread.Sleep(1000);
                }
            }
            client.Dispose();
            Content.Dispose();
            return "1";
        }
        #endregion


        #region GetDividends
        public string GetDividends(string url_get)
        {
            try
            {
                Console.WriteLine(url_get);
                var url = url_get;
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                var r = client.ExecuteAsync(request).Result.Content;

                var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
                JavaScriptSerializer? js = new JavaScriptSerializer();
                var poly_tickers = js.Deserialize<Dividends>(r);
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    var tickers = (from i in _context.StockDividends select i).AsNoTracking().ToList();
                    foreach (var tick in poly_tickers.results)
                    {
                        var find_ticker_in_database = (from i in tickers where i.ticker == tick.ticker && i.pay_date == tick.pay_date select i).FirstOrDefault();
                        if (find_ticker_in_database == null)
                        {
                            api.allinoneapi.Models.Stocks.Polygon.Dividends.Result stock = new api.allinoneapi.Models.Stocks.Polygon.Dividends.Result();
                            stock.cash_amount = tick.cash_amount;
                            stock.currency = tick.currency;
                            stock.declaration_date = tick.declaration_date;
                            stock.dividend_type = tick.dividend_type;
                            stock.ex_dividend_date = tick.ex_dividend_date;
                            stock.frequency = tick.frequency;
                            stock.pay_date = tick.pay_date;
                            stock.record_date = tick.record_date;
                            stock.ticker = tick.ticker;
                            stock.update_date = DateTime.Now;
                            tickers.Add(stock);

                            Console.WriteLine("Добавлен тикер:" + tick.ticker + " , pay_date: " + tick.pay_date);
                        }
                    }
                    _context.BulkMerge(tickers);
                    _context.BulkSaveChanges();
                }
                Content.Dispose();
                client.Dispose();
                return poly_tickers.next_url + api;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Thread.Sleep(86400000);
                return "";
            }
            finally
            {

            }
        }
        #endregion

        #region GetInstruments
        public string GetInstruments(string url_get)
        {
            try
            {
                var url = url_get;
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                var r = client.ExecuteAsync(request).Result.Content;

                var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
                JavaScriptSerializer? js = new JavaScriptSerializer();
                var poly_tickers = js.Deserialize<PolygonTickers>(r);
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    var tickers = (from i in _context.StockInstruments select i).AsNoTracking();
                    foreach (var tick in poly_tickers.results)
                    {
                        var find_ticker_in_database = (from i in tickers where i.ticker == tick.ticker && i.name == tick.name select i).FirstOrDefault();
                        if (find_ticker_in_database == null)
                        {
                            StockInstruments stock = new StockInstruments();
                            stock.market = tick.market;
                            stock.name = tick.name;
                            stock.ticker = tick.ticker;
                            stock.currency_name = tick.currency_name;
                            stock.active = tick.active;
                            stock.type = tick.type;
                            stock.composite_figi = tick.composite_figi;
                            stock.share_class_figi = tick.share_class_figi;
                            stock.locale = tick.locale;
                            stock.update_date = DateTime.Now;
                            _context.Add(stock);

                            Console.WriteLine("Добавлен тикер:" + tick.ticker + " , name: " + tick.name);
                        }
                    }
                    _context.SaveChanges();
                }
                Content.Dispose();
                client.Dispose();
                return poly_tickers.next_url + api;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Thread.Sleep(86400000);
                return "";
            }
            finally
            {
                
            }
        }
        #endregion

        #region UpdateStockDescription
        public async Task<string> UpdateStockDescription(string ticker)
        {
            try
            {
                var url = website+ "/api/Stock/GetInstrumentsDescription?ticker=" + ticker;
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                var r = client.ExecuteAsync(request).Result.Content;

                var Content = new StringContent(r.ToString(), Encoding.UTF8, "application/json");
                JavaScriptSerializer? js = new JavaScriptSerializer();
                var tick = js.Deserialize<api.allinoneapi.Models.Stocks.Polygon.Results>(r);
                await using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    var tickers = (from i in _context.StockDescription orderby i.cik select i).AsNoTracking();

                    var find_ticker_in_database = (from i in tickers where i.ticker == tick.ticker && i.name == tick.name select i).FirstOrDefault();
                    if (find_ticker_in_database == null)
                    {
                        StockDescription stock = new StockDescription();
                        stock.market = tick.market;
                        stock.name = tick.name;
                        stock.ticker = tick.ticker;
                        stock.currency_name = tick.currency_name;
                        stock.active = tick.active;
                        stock.type = tick.type;
                        stock.composite_figi = tick.composite_figi;
                        stock.share_class_figi = tick.share_class_figi;
                        stock.locale = tick.locale;
                        stock.update_date = DateTime.Now;
                        stock.primary_exchange = tick.primary_exchange;
                        stock.cik = tick.cik;
                        stock.market_cap = tick.market_cap;
                        stock.phone_number = tick.phone_number;
                        stock.address1 = tick.address.address1;
                        stock.city = tick.address.city;
                        stock.state = tick.address.state;
                        stock.postal_code = tick.address.postal_code;
                        stock.description = tick.description;
                        stock.sic_code = tick.sic_code;
                        stock.sic_description = tick.sic_description;
                        stock.ticker_root = tick.ticker_root;
                        stock.homepage_url = tick.homepage_url;
                        stock.total_employees = tick.total_employees;
                        stock.list_date = tick.list_date;
                        stock.logo_url = tick.branding.logo_url;
                        stock.icon_url = tick.branding.icon_url;
                        stock.share_class_shares_outstanding = tick.share_class_shares_outstanding;
                        stock.weighted_shares_outstanding = tick.weighted_shares_outstanding;
                        stock.round_lot=tick.round_lot;
                        stock.update_date = DateTime.Now;
                        _context.ChangeTracker.AutoDetectChangesEnabled = false;
                        await _context.AddAsync(stock);
                        _context.ChangeTracker.AutoDetectChangesEnabled = false;

                        Console.WriteLine("Добавлено описание тикера:" + tick.ticker + " , name: " + tick.name);
                    }

                    await _context.SaveChangesAsync();
                }
                Content.Dispose();
                client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Беда:" + ticker);
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    StockDescription stock = new StockDescription();
                    stock.ticker = ticker;
                    stock.update_date= DateTime.Now;
                    await _context.AddAsync(stock);
                    await _context.SaveChangesAsync();
                }

            }
            finally
            {
                
            }
            return "Ok";
        }
        #endregion

        #region DisposeCtor
        public void Dispose()
        {
           
        }
        ~Poly()
        {

        }
        #endregion
    }
}
