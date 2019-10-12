using System.Collections.Generic;

namespace QuotationDance.Collector
{
    public class TempPair
    {
        public static List<CoinPair> Huobi => new List<CoinPair>
        {
            //new CoinPair("btc", "cny"),
            //new CoinPair("btc", "ht"),
            new CoinPair("btc", "husd"),
            new CoinPair("btc", "usdt")
        };

        public static List<CoinPair> Okex => new List<CoinPair>
        {
            new CoinPair("btc", "usdk"),
            new CoinPair("btc", "usdt")
        };

        public static List<CoinPair> CoinBase => new List<CoinPair>
        {
            new CoinPair("btc", "usd"),
            new CoinPair("btc", "usdc"),
            new CoinPair("btc", "eur"),
            new CoinPair("btc", "gbp")
        };
    }
}