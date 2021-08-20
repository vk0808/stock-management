using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Serialization.Json;

namespace StockManagement
{
    public class StockAccount
    {
        // instance variables
        public double _portfolioTotal = 0;
        public string _path;

        // constructor
        public StockAccount(string path)
        {
            this._path = path;
        }


        // Method to return welcome message
        public string welcome()
        {
            return "Welcome to Stock Account Management Program\n";
        }


        // Method to display menu and get task 
        public void menu()
        {
            int task = 6;
            while (task != 1 && task != 2 && task != 3 && task != 4 && task != 5)
            {
                Console.WriteLine("\n==============================================\n");
                Console.WriteLine("Enter the task you want to perform\n1. Buy\n2. Sell\n3. View Portfolio\n4. Show total value\n5. Exit\n");
                task = int.Parse(Console.ReadLine());

                if (task == 5) // exit
                {
                    Console.WriteLine("\nExiting...");
                    break;
                }

                if (task != 1 && task != 2 && task != 3 && task != 4) // when wrong number
                {
                    Console.WriteLine("You have enterd wrong task number\n");
                }
                else // performing task
                {
                    performTask(task);
                    task = 6; // to keep inside loop
                }
            }
        }


        // Method to write to json file
        private void save(List<Stock> list)
        {
            // JSON-Serializing
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(this._path, json);

            Console.WriteLine("\nStock details has been saved successFully to JSON File.\n");
        }


        // Method to add share
        public List<Stock> addShare(List<Stock> list, string name, string symbol, double no, double price)
        {

            Stock stock = new Stock(name, symbol, no, price);
            list.Add(stock);
            return list;
        }


        // Method to read json file
        private List<Stock> readJSON()
        {
            // Check if file exists, if not then create file
            if (File.Exists(this._path) == false)
            {
                Console.WriteLine("\nThere is no file. Create a new file by adding stocks.\n");
                List<Stock> list = new List<Stock>();
                return list;
            }

            //Deserializing JSON file
            string file = File.ReadAllText(this._path);
            List<Stock> dataFile = JsonConvert.DeserializeObject<List<Stock>>(file);
            return dataFile;
        }


        // method to add shares to stock
        public void buy(double amount, string symbol, double price)
        {
            bool notFound = true;
            List<Stock> dataFile = readJSON();
            double units = roundDouble(amount / price, 4);
            foreach (var item in dataFile)
            {
                double totalAmount = 0;
                if (item._symbol == symbol)
                {
                    totalAmount = item._sharePrice * item._noOfShares + amount;
                    item._noOfShares += units;
                    item._sharePrice = totalAmount / item._noOfShares;
                    Console.WriteLine("\n***Buy successful***\n");
                    Console.WriteLine($"{units} shares of {item._name} purchased.\nTotal shares: {roundDouble(item._noOfShares, 4)}\n");
                    notFound = false;
                    break;
                }

            }
            if (notFound)
            {
                Console.WriteLine("Stock not in your portfolio. Stock will be added to your portfolio.\n");
                Console.Write("\nEnter stock name: ");
                string name = Console.ReadLine().ToLower();
                dataFile = addShare(dataFile, name, symbol, units, price);
                Console.WriteLine("\n***Buy successful***\n");

            }
            save(dataFile);
        }


        // method to subtract shares from stock
        public void sell(double amount, string symbol, double price)
        {
            List<Stock> dataFile = readJSON();
            foreach (var item in dataFile)
            {
                if (item._symbol == symbol)
                {
                    double units = roundDouble(amount / price, 4);
                    if (item._noOfShares >= units)
                    {
                        item._noOfShares -= roundDouble(amount / price, 4);
                        Console.WriteLine("\n***Sell successful***\n");
                        Console.WriteLine($"{units} shares of {item._name} sold.\nTotal shares: {roundDouble(item._noOfShares, 4)}\n");
                        save(dataFile);
                    }
                    else
                    {
                        Console.WriteLine($"Can't sell shares. {units} is more than {roundDouble(item._noOfShares, 4)}\n");
                    }
                }
            }
        }


        // method to get stock name and amount
        public string[] getCompany(string type)
        {
            Console.Write("\nEnter the Stock symbol: ");
            string symbol = Console.ReadLine().ToLower();

            Console.Write($"\nEnter the amount you want to {type}: ");
            string amount = Console.ReadLine();

            Console.Write("\nEnter share price: ");
            string price = Console.ReadLine();

            string[] array = { symbol, amount, price };
            return array;
        }


        // method to round off decimal number
        public double roundDouble(double num, int point)
        {
            return Math.Round(num, point, MidpointRounding.AwayFromZero);
        }


        // method to calculate the total value
        public double valueOf()
        {
            this._portfolioTotal = 0;
            List<Stock> dataFile = readJSON();
            foreach (var item in dataFile)
            {
                // Calulate the total value
                this._portfolioTotal += (item._noOfShares * item._sharePrice);
            }
            return roundDouble(this._portfolioTotal, 2);
        }


        // Method to display data stored in JSON file
        public void printReport()
        {
            Console.WriteLine("\nPortfolio details: \n");
            List<Stock> dataFile = readJSON();
            Console.WriteLine($"{"Stock Name",25}\t{"Symbol",10}\t{"Units",10}\t{"Price(Rs.)",10}\tValue(Rs.)");

            foreach (var item in dataFile)
            {
                // Display name, weight, price per kg
                Console.WriteLine($"{item._name,25} |\t{item._symbol.ToUpper(),10} |\t{roundDouble(item._noOfShares, 4),10} |\t{roundDouble(item._sharePrice, 4),10} |\t{roundDouble(item._noOfShares * item._sharePrice, 2)}");
            }
            Console.WriteLine($"\nTotal value : Rs. {valueOf()}\n");
        }


        // method to perform tasks
        public void performTask(int choice)
        {
            switch (choice)
            {
                case 1:
                    string[] detailsBuy = getCompany("invest");
                    string symbolBuy = detailsBuy[0];
                    double amountBuy = double.Parse(detailsBuy[1]);
                    double priceBuy = double.Parse(detailsBuy[2]);
                    buy(amountBuy, symbolBuy, priceBuy);
                    break;

                case 2:
                    string[] detailsSell = getCompany("sell");
                    string symbolSell = detailsSell[0];
                    double amountSell = double.Parse(detailsSell[1]);
                    double priceSell = double.Parse(detailsSell[2]);
                    sell(amountSell, symbolSell, priceSell);
                    break;

                case 3:
                    printReport();
                    break;

                case 4:
                    Console.WriteLine($"\nTotal value : Rs. {valueOf()}"); ;
                    break;

                default:
                    break;
            }
        }
    }
}
