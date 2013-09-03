using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace StockService
{
    public enum Tick
    {
        Up,
        Down,
        NoChange
    }

    public class StockQuote : INotifyPropertyChanged
    {
        private string _symbol;
        private double _price;
        private DateTime _date;
        private Tick _tick;
        private double _average;

        public StockQuote(string symbol)
        {
            _symbol = symbol;
        }

        public StockQuote(string symbol, double price, DateTime date, Tick tick)
        {
            _symbol = symbol;
            _price = price;
            _date = date;
            _tick = tick;
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; NotifyPropertyChanged("Symbol"); }
        }

        public double Price
        {
            get { return _price; }
            set { _price = value; NotifyPropertyChanged("Price"); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; NotifyPropertyChanged("Date"); }
        }

        public Tick Tick
        {
            get { return _tick; }
            set { _tick = value; NotifyPropertyChanged("Tick"); }
        }

        public double Average
        {
            get { return _average; }
            set { _average = value; NotifyPropertyChanged("Average"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


        /// <summary>
        /// Lowest priced quote
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static StockQuote MinPrice(StockQuote A, StockQuote B)
        {
            if (A.Price < B.Price)
                return A;
            return B;
        }

        /// <summary>
        /// Highest priced Quote
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static StockQuote MaxPrice(StockQuote A, StockQuote B)
        {
            if (A.Price > B.Price)
                return A;
            return B;
        }
    }
}
