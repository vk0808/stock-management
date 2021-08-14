using System;

namespace StockManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            StockAccount stock = new StockAccount();

            Console.WriteLine(stock.welcome());
            stock.performTask();
        }
    }
}
