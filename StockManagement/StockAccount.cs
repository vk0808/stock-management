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

        public string _path = @"..\..\..\Portfolio.json";

        // Method to return welcome message
        public string welcome()
        {
            return "Welcome to Stock Account Management Program\n";
        }


        // Method to write to json file
        private void writeJSON(string path, List<Stock> list)
        {
            // JSON-Serializing
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);

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
        private List<Stock> readJSON(string path)
        {
            // Check if file exists, if not then create file
            if (File.Exists(path) == false)
            {
                Console.WriteLine("\nThere is no file. New json file created\n");
                List<Stock> list = new List<Stock>();
                list = addShare(list, "tcs", 634, 3360);
                list = addShare(list, "tata consumer", 1340, 777);
                list = addShare(list, "reliance industries", 956, 2117.30);
                writeJSON(path, list);
            }

            //Deserializing JSON file
            string file = File.ReadAllText(path);
            List<Stock> dataFile = JsonConvert.DeserializeObject<List<Stock>>(file);
            return dataFile;
        }

        // Method to display data stored in JSON file
        public void displayJSON(List<Stock> dataFile)
        {
            double portfolioTotal = 0;
            foreach (var item in dataFile)
            {
                // Display name, weight, price per kg
                Console.WriteLine($"Stock Name : {item._name}\nNo. of Shares : {item._noOfShares} units\nShare Price: Rs. {item._sharePrice}");

                // Calulate the total value
                portfolioTotal += (item._noOfShares * item._sharePrice);
                Console.WriteLine($"{item._name}'s total value : Rs. {(item._noOfShares * item._sharePrice)}\n");
                Console.WriteLine("--------------------------------------------\n");
            }
            Console.WriteLine($"\nTotal value of the portfolio = Rs. {portfolioTotal}\n");
        }


        public void performTask()
        {
            List<Stock> dataFile = readJSON(this._path);
            displayJSON(dataFile);
        }
    }
}
