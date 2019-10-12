using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace QuotationDance.Collector
{
    public class WebSocketCollectService : IHostedService
    {
        private readonly string _exchange;
        private readonly IDatabase _db;
        private readonly ILogger _logger;
        private readonly WebSocketConfig _config;
        private readonly Dictionary<string, WebSocketCollector> _collectors;

        public WebSocketCollectService(ILogger logger, string exchange)
        {
            _logger = logger;
            _exchange = exchange;
            _config = GetConfig(exchange);
            var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");

            _db = redis.GetDatabase();
            _collectors = new Dictionary<string, WebSocketCollector>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var coins = GetCoins(_exchange);

            foreach (var coin in coins)
            {
                var pairs = GetPairs(_exchange, coin);

                foreach (var pair in pairs)
                {
                    var collector = new WebSocketCollector(_config, _exchange, coin, pair, _db, _logger);
                    _collectors[$"{coin}_{pair.Pair1}_{pair.Pair2}"] = collector;
                    collector.Start();
                }
            }

            _logger.LogInformation($"[{_exchange}]Web Socket is started.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[{_exchange}]Web Socket is stopping.");
            return Task.CompletedTask;
        }

        private WebSocketConfig GetConfig(string exchange)
        {
            if (exchange == "huobipro")
            {
                return TempConfig.Huobi;
            }

            if (exchange == "okex")
            {
                return TempConfig.Okex;
            }

            if (exchange == "coinbasepro")
            {
                return TempConfig.CoinBase;
            }

            return null;
        }

        private List<string> GetCoins(string exchange)
        {
            return TempCoin.Coins;
        }

        private List<CoinPair> GetPairs(string exchange, string coin)
        {
            if (exchange == "huobipro")
            {
                return TempPair.Huobi;
            }

            if (exchange == "okex")
            {
                return TempPair.Okex;
            }

            if (exchange == "coinbasepro")
            {
                return TempPair.CoinBase;
            }

            return null;
        }
    }
}