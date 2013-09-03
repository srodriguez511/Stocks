using System;
using StockService;

namespace StockTicker.Design
{
    public class DesignStockProvider : IStockProvider
    {
        public void Subscribe(string symbol)
        {
            throw new NotImplementedException();
        }

        public IObservable<StockQuote> StockQuoteStream
        {
            get { throw new NotImplementedException(); }
        }
    }
}