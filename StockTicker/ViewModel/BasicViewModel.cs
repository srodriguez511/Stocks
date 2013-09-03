using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using StockService;
using GalaSoft.MvvmLight.Command;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System;
using StockTicker.ViewModel;

namespace StockTicker
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BasicViewModel : ViewModelBase
    {
        private readonly IStockProvider _dataService;

        /// <summary>
        /// Initializes a new instance of the BasicViewModel class.
        /// </summary>
        public BasicViewModel(IStockProvider dataService)
        {
            _dataService = dataService;
            Quotes = new ObservableCollection<StockQuote>();
            _dataService.StockQuoteStream.ObserveOnDispatcher().Subscribe(SubscribeCallback);
        }


        /// <summary>
        /// Handle a new quote coming in
        /// </summary>
        /// <param name="quote"></param>
        private void SubscribeCallback(StockQuote quote)
        {
            Quotes.Insert(0, quote);
        }

        /// <summary>
        /// The stock quotes
        /// </summary>
        public ObservableCollection<StockQuote> Quotes { get; set; }
    }
}