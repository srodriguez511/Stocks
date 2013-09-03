using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using StockService;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Reactive.Linq;

namespace StockTicker.ViewModel
{
    public class GroupViewModel : ViewModelBase
    {
        private readonly IStockProvider _dataService;

        public GroupViewModel(IStockProvider dataService)
        {
            _dataService = dataService;
            Quotes = new ObservableCollection<StockQuote>();

            var singleStream = _dataService.StockQuoteStream.ObserveOnDispatcher();
            singleStream.Subscribe(UpdateQuoteCallback());
        }

        /// <summary>
        /// Get a new quote... Add or update old 
        /// </summary>
        /// <returns></returns>
        private Action<StockQuote> UpdateQuoteCallback()
        {
            return newQuote =>
            {
                var oldQuote = Quotes.FirstOrDefault(item => item.Symbol.Equals(newQuote.Symbol));
                if (oldQuote == null)
                {
                    Quotes.Insert(0, newQuote);
                }
                else
                {
                    oldQuote.Price = newQuote.Price;
                    oldQuote.Tick = newQuote.Tick;
                    oldQuote.Date = newQuote.Date;
                }
            };
        }

        /// <summary>
        /// The stock quotes
        /// </summary>
        public ObservableCollection<StockQuote> Quotes { get; set; }
    }
}
