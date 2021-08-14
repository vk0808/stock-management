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
        public double _portfolioTotal = 0;
        public string _path;

        public StockAccount(string path)
        {
            this._path = path;
        }

        // Method to return welcome message
        public string welcome()
        {
            return "Welcome to Stock Account Management Program\n";
        }


        // Method to write to json file
        private void save(List<Stock> list)
        {
            // JSON-Serializing
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(this._path, json);

            Console.WriteLine("\nStock details has been added successFully to JSON File.\n");
        }


        // Method to add share
        public List<Stock> addShare(List<Stock> list, string name, double no, double price)
        {

            Stock stock = new Stock(name, no, price);
            list.Add(stock);
            return list;
        }


        // Method to read json file
        private List<Stock> readJSON()
        {
            // Check if file exists, if not then create file
            if (File.Exists(this._path) == false)
            {
                Console.WriteLine("\nThere is no file. New json file created\n");
                List<Stock> list = new List<Stock>();
                list = addShare(list, "tcs", 634, 3360);
                list = addShare(list, "tata consumer", 1340, 777);
                list = addShare(list, "reliance industries", 956, 2117.30);
                save(list);
            }

            //Deserializing JSON file
            string file = File.ReadAllText(this._path);
            List<Stock> dataFile = JsonConvert.DeserializeObject<List<Stock>>(file);
            return dataFile;
        }


        // Method to display data stored in JSON file
        public void printReport()
        {
            List<Stock> dataFile = readJSON();
            foreach (var item in dataFile)
            {
                // Display name, weight, price per kg
                Console.WriteLine($"Stock Name : {item._name}\nNo. of Shares : {item._noOfShares} units\nShare Price: Rs. {item._sharePrice}");

                Console.WriteLine($"{item._name}'s total value : Rs. {(item._noOfShares * item._sharePrice)}\n");
                Console.WriteLine("--------------------------------------------\n");
            }

            Console.WriteLine($"\nTotal value of the portfolio = Rs. {valueOf()}\n");
        }


        // method to calculate the total value
        public double valueOf()
        {
            List<Stock> dataFile = readJSON();
            foreach (var item in dataFile)
            {
                // Calulate the total value
                this._portfolioTotal += (item._noOfShares * item._sharePrice);
            }
            return this._portfolioTotal;
        }


        // method to add shares to stock
        public void buy(double amount, string name)
        {
            bool notFound = true;
            List<Stock> dataFile = readJSON();
            foreach (var item in dataFile)
            {
                if (item._name == name)
                {
                    item._noOfShares += amount;
                    Console.WriteLine("Buy successful\n");
                    Console.WriteLine($"{amount} shares of {item._name} purchased.\nTotal shares: {item._noOfShares}\n");
                    notFound = false;
                    break;
                }
                
            }
            if (notFound)
            {
                Console.WriteLine("Stock not in your portfolio. Add stock to your portfolio.\nEnter share price: ");
                double price = double.Parse(Console.ReadLine());
                Console.WriteLine("Enter no of shares you want to buy:");
                double num = double.Parse(Console.ReadLine());
                dataFile = addShare(dataFile, name, num, price);
            }
            save(dataFile);
        }


        // method to subtract shares from stock
        public void sell(double amount, string name)
        {
            List<Stock> dataFile = readJSON();
            foreach (var item in dataFile)
            {
                if (item._name == name)
                {
                    if (item._noOfShares >= amount)
                    {

                        item._noOfShares -= amount;
                        Console.WriteLine("Sell successful\n");
                        Console.WriteLine($"{amount} shares of {item._name} sold.\nTotal shares: {item._noOfShares}\n");
                    }
                    else
                    {
                        Console.WriteLine($"Can't sell shares. {amount} is more than {item._noOfShares}\n");
                    }
                }
            }
            save(dataFile);
        }

        // method to perform tasks
        public void performTask()
        {
            printReport();
            Console.WriteLine("Enter no. of shares you want to buy");
            double amt = double.Parse(Console.ReadLine());
            sell(amt, "tata consumer");
            printReport();
        }
    }
}
