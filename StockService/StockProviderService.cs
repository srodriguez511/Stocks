using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Reactive.Concurrency;

namespace StockService
{
    public class StockProviderService : IStockProvider
    {
        /// <summary>
        /// List of already subscribed symbols
        /// </summary>
        private readonly ConcurrentDictionary<string, double> _subscribedSymbols = new ConcurrentDictionary<string, double>();

        /// <summary>
        /// Random generator
        /// </summary>
        private Random _rand = new Random();

        public StockProviderService()
        {
            StartQuoteStream();
        }

        /// <summary>
        /// Provides generated quotes on the requested subscriptions
        /// </summary>
        private void StartQuoteStream()
        {
            var rxTimer = Observable.Interval(TimeSpan.FromMilliseconds(1000));
            var res = rxTimer.SelectMany(val =>
            {
                var current = _subscribedSymbols.ToList();
                if (current.Count <= 0)
                {
                    return new List<StockQuote>();
                }

                //Pick random stocks to update...
                var randNumStocksToUpdate = _rand.Next(0, current.Count + 1);
                //Generate list of possible numbers to choose from
                var poolNumbers = Observable.Range(0, randNumStocksToUpdate).ToEnumerable().ToList();

                //Pick those random numbers
                int numSelected = 0;
                var result = new List<StockQuote>();
                while (numSelected < randNumStocksToUpdate)
                {
                    int index = _rand.Next(poolNumbers.Count());
                    poolNumbers.RemoveAt(index);
                    numSelected++;

                    var oldPrice = current[index].Value;
                    var newPrice = Math.Round(oldPrice + (_rand.NextDouble() * 2.0 - 1.0), 4);

                    _subscribedSymbols[current[index].Key] = newPrice;

                    Tick tick;
                    if (newPrice < oldPrice)
                        tick = Tick.Down;
                    else if (newPrice > oldPrice)
                        tick = Tick.Up;
                    else
                        tick = Tick.NoChange;

                    result.Add(new StockQuote(current[index].Key, newPrice, DateTime.Now, tick));
                }
                return result;
            }).Publish();
            res.Connect();
            StockQuoteStream = res.AsObservable();


            //Using old school timers
            //var stream = Observable.Create<StockQuote>(obs =>
            //{
            //    var timer = new System.Timers.Timer();
            //    timer.Interval = 1000;
            //    timer.Elapsed += (s, e) =>
            //    {
            //        var current = _subscribedSymbols.ToList();
            //        for (int i = 0; i < current.Count; i++)
            //        {
            //            obs.OnNext(new StockQuote(current[i], 1.0, 1, DateTime.Now, Tick.Up));
            //        }
            //    };
            //    timer.Start();
            //    return timer;
            //});
            //StockQuoteStream = stream.AsObservable();
        }

        /// <summary>
        /// Subscribe to a given quote
        /// </summary>
        /// <param name="symbol"></param>
        public void Subscribe(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                return;

            if (_subscribedSymbols.ContainsKey(symbol))
                return;

            var startPrice = Math.Round(_rand.NextDouble() * 0.1, 4);
            _subscribedSymbols.TryAdd(symbol, startPrice);
        }

        public IObservable<StockQuote> StockQuoteStream { get; private set; }
    }
}
