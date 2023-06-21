using CryptoAPI.Exchanges;
using CryptoAPI.Stocks.Polygon;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Xml;
using System.Threading;
using CryptoAPI.Tinkoff;
using CryptoAPI.Polygon;
using CryptoAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI
{
    internal class Program
    {
        private static string api = "&apiKey=";
        static void Main(string[] args)
        {
            XmlDocument xApi = new XmlDocument();
            xApi.Load("api.xml");
            XmlElement? xRootApi = xApi.DocumentElement;
            if (xRootApi != null)
            {
                foreach (XmlElement xnode in xRootApi)
                {
                    api = api + xnode.ChildNodes[1].InnerText;
                }
            }
            Thread BinanceUpdatePricesThread = new Thread(BinanceUpdatePrices);
            Thread BinanceUpdatePairsThread = new Thread(BinanceUpdatePairs);
            Thread BinanceUpdateKandlesThread = new Thread(BinanceUpdateKandles);
            Thread TinkoffUpdateBonds = new Thread(TinkoffUpdateBondsService);
            Thread TinkoffUpdateEtfs = new Thread(TinkoffUpdateEtfsService);
            Thread TinkoffUpdateStocks = new Thread(TinkoffUpdateStocksService);
            Thread UpdateStockInstrument = new Thread(UpdateStockInstruments);
            Thread UpdateDividendsThread = new Thread(UpdateDividendInstruments);
            Thread UpdateDividendsTinkoffThread = new Thread(UpdateDividendTinkoffInstruments);
            Thread UpdateStockInstrumentDescriptions = new Thread(UpdateStockInstrumentsDescription);
            Thread GetNews = new Thread(UpdateNews);
            //Thread Test = new Thread(TestNew);
            XmlDocument xDoc = new XmlDocument();
            
            xDoc.Load("settings.xml");

            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    XmlNode? attr = xnode.Attributes.GetNamedItem("crypto");
                    Console.WriteLine(attr?.Value);

                    #region XML_PARAMS

                    if (xnode.ChildNodes[0].InnerText == "binance" && xnode.ChildNodes[1].InnerText == "UpdatePrices")
                    {
                        BinanceUpdatePricesThread.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "polygon" && xnode.ChildNodes[1].InnerText == "UpdateKandles")
                    {
                        BinanceUpdateKandlesThread.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "Tinkoff" && xnode.ChildNodes[1].InnerText == "UpdateBonds")
                    {
                        TinkoffUpdateBonds.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "Tinkoff" && xnode.ChildNodes[1].InnerText == "UpdateEtfs")
                    {
                        TinkoffUpdateEtfs.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "Tinkoff" && xnode.ChildNodes[1].InnerText == "UpdateStocks")
                    {
                        TinkoffUpdateStocks.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "polygon" && xnode.ChildNodes[1].InnerText == "UpdateInstruments")
                    {
                        UpdateStockInstrument.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "polygon" && xnode.ChildNodes[1].InnerText == "UpdateInstrumentsDescription")
                    {
                        UpdateStockInstrumentDescriptions.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "polygon" && xnode.ChildNodes[1].InnerText == "GetNews")
                    {
                        GetNews.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "binance" && xnode.ChildNodes[1].InnerText == "UpdateInstruments")
                    {
                        BinanceUpdatePairsThread.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "polygon" && xnode.ChildNodes[1].InnerText == "UpdateDividends")
                    {
                        UpdateDividendsThread.Start();
                    }
                    if (xnode.ChildNodes[0].InnerText == "Tinkoff" && xnode.ChildNodes[1].InnerText == "UpdateDividendTinkoffInstruments")
                    {
                        UpdateDividendsTinkoffThread.Start();
                    }
                    #endregion

                }
            }
        }

        #region BinanceUpdateKandles
        private static void BinanceUpdateKandles(object? obj)
        {
            while (true)
            {
                Stock st = new Stock();
                st.GetKandles();
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(300000);
            }
        }
        #endregion

        #region UpdateDividendInstruments
        private static void UpdateDividendInstruments(object? obj)
        {
            string first_page = "https://api.polygon.io/v3/reference/dividends?limit=1000";
            string url = "";
            while (true)
            {
                Poly st = new Poly();
                if (url.Length == 0)
                {
                    url = st.GetDividends(first_page + api);
                }
                else
                {
                    url = st.GetDividends(url);
                }
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //Thread.Sleep(300000);
            }
        }
        #endregion

        #region UpdateDividendTinkoffInstruments
        private static void UpdateDividendTinkoffInstruments(object? obj)
        {
            while (true)
            {
                Poly st = new Poly();
                st.GetDividendsTinkoff();
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(86400000);
            }
        }
        #endregion

        #region UpdateStockInstruments
        private static void UpdateStockInstruments(object? obj)
        {
            string first_page = "https://api.polygon.io/v3/reference/tickers?active=true&limit=1000";
            string url = "";
            while (true)
            {
                Poly st = new Poly();
                if (url.Length == 0)
                {
                    url = st.GetInstruments(first_page + api);
                }
                else
                {
                    url = st.GetInstruments(url);
                }
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //Thread.Sleep(300000);
            }
        }
        #endregion

        #region TinkoffUpdateBondsService
        private static void TinkoffUpdateBondsService(object? obj)
        {
            while (true)
            {
                TinkoffData st = new TinkoffData();
                st.UpdateBonds();
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(86400000);
            }
        }
        #endregion

        #region TinkoffUpdateStocksService
        private static void TinkoffUpdateStocksService(object? obj)
        {
            while (true)
            {
                TinkoffData st = new TinkoffData();
                st.UpdateStocks();
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(86400000);
            }
        }
        #endregion

        #region UpdateNews
        private static void UpdateNews(object? obj)
        {
            while (true)
            {
                Stock st = new Stock();
                st.GetNews();
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(300000);
            }
        }
        #endregion

        #region TinkoffUpdateEtfsService
        private static void TinkoffUpdateEtfsService(object? obj)
        {
            while (true)
            {
                TinkoffData st = new TinkoffData();
                st.UpdateEtfs();
                st.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(86400000);
            }
        }
        #endregion

        #region UpdateStockInstrumentsDescription
        private static void UpdateStockInstrumentsDescription(object? obj)
        {
            while (true)
            {
                Poly st = new Poly();
                using (CryptoAPIContext _context = new CryptoAPIContext())
                {
                    var tickers = (from i in _context.StockInstruments 
                                   where !(from o in _context.StockDescription select o.ticker).Contains(i.ticker)
                                   select i).AsNoTracking().ToHashSet();
                    foreach (var t in tickers)
                    {
                        Console.WriteLine("Ищу: " + t.ticker);
                        st.UpdateStockDescription(t.ticker);
                        st.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                Thread.Sleep(86400000);
            }
        }
        #endregion

        #region BinanceUpdatePrices
        private static void BinanceUpdatePrices(object? obj)
        {
            while (true)
            {
                Binance bn = new Binance();
                bn.UpdatePrices();
                bn.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        #endregion

        #region BinanceUpdatePairs
        private static void BinanceUpdatePairs(object? obj)
        {
            while (true)
            {
                Binance bn = new Binance();
                bn.UpdatePairs();
                bn.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(86400000);
            }
        }
        #endregion

        //#region Test
        //private static void TestNew(object? obj)
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("TEST");
        //        Thread.Sleep(1000);
        //    }
        //}
        //#endregion

    }
}