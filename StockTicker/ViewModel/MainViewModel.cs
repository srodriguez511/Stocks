using GalaSoft.MvvmLight;
using StockService;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System;
using System.Reactive.Linq;

namespace StockTicker.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IStockProvider _dataService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IStockProvider dataService)
        {
            _dataService = dataService;
            BVM = new BasicViewModel(dataService);
            GVM = new GroupViewModel(dataService);
            GHVM = new GroupHistoryViewModel(dataService);

            var loStreamQuote = _dataService.StockQuoteStream.ObserveOnDispatcher()
                .Scan((quoteA, quoteB) => StockQuote.MinPrice(quoteA, quoteB)).DistinctUntilChanged();
            var hiStreamQuote = _dataService.StockQuoteStream.ObserveOnDispatcher()
                .Scan((quoteA, quoteB) => StockQuote.MaxPrice(quoteA, quoteB)).DistinctUntilChanged();

            loStreamQuote.Subscribe(quote => LoQuote = quote);
            hiStreamQuote.Subscribe(quote => HiQuote = quote);
        }

        public BasicViewModel BVM { get; set; }
        public GroupViewModel GVM { get; set; }
        public GroupHistoryViewModel GHVM { get; set; }

        /// <summary>
        /// Symbol just requested
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Lowest quote
        /// </summary>
        private StockQuote _loQuote;
        public StockQuote LoQuote 
        {
            get
            {
                return _loQuote;
            }
            set
            {
                _loQuote = value;
                RaisePropertyChanged(() => LoQuote);
            }
        }

        /// <summary>
        /// Highest quote
        /// </summary>
        private StockQuote _hiQuote;
        public StockQuote HiQuote
        {
            get
            {
                return _hiQuote;
            }
            set
            {
                _hiQuote = value;
                RaisePropertyChanged(() => HiQuote);
            }
        }

        /// <summary>
        /// Subscribed to new symbol
        /// </summary>
        private RelayCommand _subscribeCommand;
        public RelayCommand SubscribeCommand
        {
            get
            {
                return _subscribeCommand
                ?? (_subscribeCommand = new RelayCommand(
                () =>
                {
                    if (!string.IsNullOrEmpty(Symbol))
                    {
                        _dataService.Subscribe(Symbol);
                    }
                }));
            }
        }
    }
}