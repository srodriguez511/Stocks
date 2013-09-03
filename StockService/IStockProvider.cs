using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockService
{
    public interface IStockProvider
    {
        void Subscribe(string symbol);
        IObservable<StockQuote> StockQuoteStream { get; }
    }
}
