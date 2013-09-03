using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockService;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StockTicker.ViewModel
{
    public class StockQuoteGroup : INotifyPropertyChanged
    {
        public StockQuoteGroup(string symbol)
        {
            Symbol = symbol;
            Quotes = new List<StockQuote>();

            
        }

        public string Symbol { get; private set; }

        /// <summary>
        /// Latest quote for this group
        /// </summary>
        private StockQuote _latest;
        public StockQuote Latest 
        {
            get
            {
                return _latest;
            }
            set
            {
                _latest = value;
                OnPropertyChanged("Latest");
            }
        }

        /// <summary>
        /// All the quotes for a particular group
        /// </summary>
        public List<StockQuote> Quotes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
