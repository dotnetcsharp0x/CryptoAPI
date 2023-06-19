using api.allinoneapi.Models;
using api.allinoneapi.Models.Stocks.Polygon;
using CryptoAPI.Data;
using CryptoAPI.Migrations;
using Z.EntityFramework.Extensions.EFCore;
using RepoDb;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using api.allinoneapi.Models.Stocks.Polygon.News;

namespace CryptoAPI.Polygon
{
    public class Stock : IDisposable
    {
        //static string website = "https://hungryapi.ru";
        static string website = "https://localhost:443";
        public Stock() { }

        #region Polygon GetKandles
        public async Task<string> GetKandles()
        {
            try
            {
                await using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    try
                    {
                        string url = website + "/api/Stock/GetKandles";
                        var client = new RestClient(url);
                        var request = new RestRequest(url, Method.Get);
                        request.AddHeader("Content-Type", "application/json");
                        var r = client.ExecuteAsync(request).Result.Content;
                        if (r != null)
                        {
                            var stock_kandles = JsonSerializer.Deserialize<List<Binance_CryptoKandles>>(r);
                            var alreadyindatabase = (from i in _context.CryptoKandles select i).AsNoTracking().ToList();
                            foreach (var cr in stock_kandles)
                            {
                                var find_in_database = (from i in alreadyindatabase
                                                        
                                                        where i.symbol.Equals(cr.symbol)
                                                        select i).FirstOrDefault();

                                if (find_in_database != null)
                                {
                                    foreach (var s in alreadyindatabase.Where(r=>r.symbol==cr.symbol))
                                    {
                                        
                                        s.openTime = cr.openTime;
                                        s.openPrice = cr.openPrice;
                                        s.highPrice = cr.highPrice;
                                        s.lowPrice = cr.lowPrice;
                                        s.closePrice = cr.closePrice;
                                        s.volume = cr.volume;
                                        s.closeTime = cr.closeTime;
                                        s.update_date = DateTime.Now;
                                        s.source = "binance";
                                    }
                                    //await _context.BulkUpdateAsync(find_in_database);
                                    //_context.Entry(find_in_database).State = EntityState.Modified;
                                }
                                else
                                {
                                    find_in_database = new Binance_CryptoKandles();
                                    find_in_database.openTime = cr.openTime;
                                    find_in_database.openPrice = cr.openPrice;
                                    find_in_database.highPrice = cr.highPrice;
                                    find_in_database.lowPrice = cr.lowPrice;
                                    find_in_database.closePrice = cr.closePrice;
                                    find_in_database.volume = cr.volume;
                                    find_in_database.closeTime = cr.closeTime;
                                    find_in_database.symbol = cr.symbol;
                                    find_in_database.source = "binance";
                                    find_in_database.update_date = DateTime.Now;
                                    alreadyindatabase.Add(find_in_database);
                                }
                            }
                            await _context.BulkMergeAsync(alreadyindatabase);
                            await _context.BulkSaveChangesAsync();
                            Console.WriteLine("Stock price updated!");
                        }
                        //Thread.Sleep(300000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(1000);
                    }
                    finally
                    {

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                //DateTime now2 = DateTime.Now;
                //TimeSpan ts = now2.Subtract(now);
                //Console.WriteLine(ts.TotalMilliseconds);
            }
            return "Ok";
        }
        #endregion

        #region GetNews

        public async Task<string> GetNews()
        {
            try
            {
                await using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    try
                    {
                        string url = website + "/api/Stock/GetNews?limit=1000";
                        var client = new RestClient(url);
                        var request = new RestRequest(url, Method.Get);
                        request.AddHeader("Content-Type", "application/json");
                        var r = client.ExecuteAsync(request).Result.Content;
                        if (r != null)
                        {
                            var stock_kandles = JsonSerializer.Deserialize<List<InstrumentsNews>>(r);
                            var alreadyindatabase = (from i in _context.InstrumentsNews select new InstrumentsNews()
                            {
                                publishername=i.publishername,
                                publisherhomepage_url=i.publisherhomepage_url,
                                publisherfavicon_url=i.publisherfavicon_url,
                                publisherlogo_url=i.publisherlogo_url,
                                published_utc=i.published_utc,
                                title=i.title,
                                author=i.author,
                                article_url=i.article_url,
                                description=i.description,
                                amp_url=i.amp_url,
                                update_date=i.update_date,
                                Id=i.Id
                            }).AsNoTracking().ToList();
                            var ticker_to_news = (from k in _context.TickerToNews select k).AsNoTracking().ToList();
                            foreach (var cr in stock_kandles)
                            {
                                var find_in_database = (from i in alreadyindatabase

                                                        where i.article_url.Equals(cr.article_url)
                                                        select i).FirstOrDefault();

                                if (find_in_database != null)
                                {
                                    foreach (var s in alreadyindatabase.Where(r => r.article_url == cr.article_url))
                                    {

                                        s.publishername = cr.publishername;
                                        s.publisherhomepage_url = cr.publisherhomepage_url;
                                        s.publisherlogo_url = cr.publisherlogo_url;
                                        s.publisherfavicon_url = cr.publisherfavicon_url;
                                        s.title = cr.title;
                                        s.author = cr.author;
                                        s.published_utc = cr.published_utc;
                                        s.article_url = cr.article_url;
                                        s.image_url = cr.image_url;
                                        s.description = cr.description;
                                        s.amp_url = cr.amp_url;
                                        s.update_date = DateTime.Now;
                                        foreach (var u in cr.tickers)
                                        {
                                            var tickernews = (from d in ticker_to_news where d.Url == cr.article_url && d.InstrumentsNewsId == s.Id select d).First();
                                            tickernews.Url = cr.article_url;
                                            tickernews.ticker = u.ticker;
                                            tickernews.InstrumentsNews = s;
                                            //ticker_to_news.Add(tickernews);
                                        }
                                    }
                                }
                                else
                                {
                                    find_in_database = new InstrumentsNews();
                                    find_in_database.publishername = cr.publishername;
                                    find_in_database.publisherhomepage_url = cr.publisherhomepage_url;
                                    find_in_database.publisherlogo_url = cr.publisherlogo_url;
                                    find_in_database.publisherfavicon_url = cr.publisherfavicon_url;
                                    find_in_database.title = cr.title;
                                    find_in_database.author = cr.author;
                                    find_in_database.published_utc = cr.published_utc;
                                    find_in_database.article_url = cr.article_url;
                                    find_in_database.image_url = cr.image_url;
                                    find_in_database.description = cr.description;
                                    find_in_database.amp_url = cr.amp_url;
                                    find_in_database.update_date = DateTime.Now;
                                    find_in_database.tickers = cr.tickers;
                                    alreadyindatabase.Add(find_in_database);
                                    foreach (var u in cr.tickers)
                                    {
                                        var tickernews = new TickerToNews();
                                        tickernews.Url = cr.article_url;
                                        tickernews.ticker = u.ticker;
                                        tickernews.InstrumentsNews = find_in_database;
                                        ticker_to_news.Add(tickernews);
                                    }
                                }
                                
                                
                                
                                //ticker_to_news.Add((from u in cr.tickers select new TickerToNews() { 
                                //    ticker = u
                                //    ,Url=cr.article_url
                                //}));
                            }
                            await _context.BulkMergeAsync(alreadyindatabase);
                            await _context.BulkMergeAsync(ticker_to_news);
                            await _context.BulkSaveChangesAsync();
                            Console.WriteLine("News updated!");
                        }
                        //Thread.Sleep(300000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(1000);
                    }
                    finally
                    {

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                //DateTime now2 = DateTime.Now;
                //TimeSpan ts = now2.Subtract(now);
                //Console.WriteLine(ts.TotalMilliseconds);
            }
            return "Ok";
        }

        #endregion

        #region DisposeCtor
        ~Stock() { }

        public void Dispose()
        {

        }

        #endregion
    }
}
