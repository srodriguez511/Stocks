using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using StockService;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace StockTicker.ViewModel
{
    public class GroupHistoryViewModel : ViewModelBase
    {
        private readonly IStockProvider _dataService;

        public GroupHistoryViewModel(IStockProvider dataService)
        {
            _dataService = dataService;

            StockGroups = new ObservableCollection<StockQuoteGroup>();

            var groupingsStream = from quote in _dataService.StockQuoteStream.ObserveOnDispatcher()
                                  group quote by quote.Symbol into company
                                  let grp = new StockQuoteGroup(company.Key)
                                  from quote in company
                                  select new
                                  {
                                      Group = grp,
                                      Quote = quote
                                  };

            var res =  groupingsStream.Select(item =>
            {
                //Add it to the list of all groups if it doesn't exist
                if (!StockGroups.Contains(item.Group))
                {
                    StockGroups.Insert(0, item.Group);
                }
                item.Group.Quotes.Insert(0, item.Quote);
                item.Group.Latest = item.Quote;

                return item;
            });
            res.Subscribe();
        }

        /// <summary>
        /// Group of stocks
        /// </summary>
        public ObservableCollection<StockQuoteGroup> StockGroups { get; set; }
    }
}
