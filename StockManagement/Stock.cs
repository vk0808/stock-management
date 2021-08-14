using System;
using System.Collections.Generic;
using System.Text;

namespace StockManagement
{
    public class Stock
    {
        public string _name;
        public double _noOfShares;
        public double _sharePrice;

        public Stock(string name, double noOfShares, double sharePrice)
        {
            this._name = name;
            this._noOfShares = noOfShares;
            this._sharePrice = sharePrice;
        }
    }
}
