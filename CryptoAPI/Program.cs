using CryptoAPI.Exchanges;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace CryptoAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                Console.WriteLine($"The app started with next parameters:");
                Console.WriteLine($"Exchange: {args[0]}, method: {args[1]}");
                if (args[0] == "-binance" && args[1] == "-UpdatePrices")
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
                else if (args[0] == "-binance" && args[1] == "-UpdatePairs")
                {
                    int currentDay = DateTime.Now.Day - 1; // Выполнение раз в сутки
                    while (true)
                    {
                        if (currentDay != DateTime.Now.Day)
                        {
                            currentDay = DateTime.Now.Day;
                            Binance bn = new Binance();
                            bn.UpdatePairs();
                            bn.Dispose();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                        }
                    }
                }
                else if (args[0] == "-binance" && args[1] == "-GetKandles")
                {
                    while (true)
                    {
                        Binance bn = new Binance();
                        bn.GetKandles();
                        bn.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                else if (args[0] == "okex" || args[1] == "-UpdatePrices")
                {
                    Console.WriteLine("Okex");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Missing right parameter!");
                    Console.WriteLine("Press any key to close the app!");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Without parameters!");
                int currentDay = DateTime.Now.Minute-1; // Выполнение раз в минуту
                while (true)
                {
                    if (currentDay != DateTime.Now.Minute)
                    {
                        currentDay = DateTime.Now.Minute;
                        Binance bn = new Binance();
                        bn.GetKandles();
                        bn.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
            }
        }
    }
}