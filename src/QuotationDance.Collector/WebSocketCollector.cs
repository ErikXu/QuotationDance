using System;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuotationDance.Collector.Decompress;
using QuotationDance.Collector.Parse;
using StackExchange.Redis;
using SuperSocket.ClientEngine;
using SuperSocket.ClientEngine.Proxy;
using WebSocket4Net;

namespace QuotationDance.Collector
{
    public class WebSocketCollector
    {
        private readonly IDatabase _db;
        private readonly ILogger _logger;
        private readonly WebSocketConfig _config;
        private readonly WebSocket _webSocket;
        private readonly string _exchange;
        private readonly string _coin;
        private readonly CoinPair _pair;

        public WebSocketCollector(WebSocketConfig config, string exchange, string coin, CoinPair pair, IDatabase db, ILogger logger)
        {
            _config = config;
            _exchange = exchange;
            _coin = coin;
            _pair = pair;
            _db = db;
            _logger = logger;
            _webSocket = new WebSocket(_config.Url);
        }

        public void Start()
        {
            _webSocket.Opened += WebSocket_Opened;
            _webSocket.Closed += WebSocket_Closed;
            _webSocket.Error += WebSocket_Error;
            _webSocket.DataReceived += WebSocket_DataReceived;
            _webSocket.MessageReceived += WebSocket_MessageReceived;

            if (_config.Proxy != null)
            {
                _webSocket.Proxy = new HttpConnectProxy(new IPEndPoint(IPAddress.Parse(_config.Proxy.Host), _config.Proxy.Port));
            }

            _webSocket.Open();
        }

        public void Stop()
        {
            _webSocket.Dispose();
        }

        private void WebSocket_Opened(object sender, EventArgs e)
        {
            switch (_config.Request.PairFormat)
            {
                case RequestPairFormat.Lower:
                    _pair.Pair1 = _pair.Pair1.ToLower();
                    _pair.Pair2 = _pair.Pair2.ToLower();
                    break;
                case RequestPairFormat.Upper:
                    _pair.Pair1 = _pair.Pair1.ToUpper();
                    _pair.Pair2 = _pair.Pair2.ToUpper();
                    break;
            }

            var request = _config.Request.Pattern.Replace("{pair1}", _pair.Pair1).Replace("{pair2}", _pair.Pair2);
            _webSocket.Send(request);
        }

        private void WebSocket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            var type = _config.Response.Type.Trim().ToLower();
            if (type == "string")
            {
                return;
            }

            IDecompress decompress = new GZipDecompress();

            switch (type)
            {
                case "gzip":
                    decompress = new GZipDecompress();
                    break;
                case "deflate":
                    decompress = new DeflateDecompress();
                    break;
            }

            var content = decompress.Decompress(e.Data);
            WebSocket_MessageReceived(sender, new MessageReceivedEventArgs(content));
        }

        private void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                Quotation quotation = null;
                switch (_config.Response.ParseType.ToLower())
                {
                    case "json":
                        var parse = new JsonParse();
                        quotation = parse.Parse(e.Message, _config.Response.JsonParseConfig);
                        break;
                }

                if (quotation != null)
                {
                    HashEntry[] hash = {
                        new HashEntry($"{_exchange}_{_pair.Pair1.ToLower()}_{_pair.Pair2.ToLower()}", JsonConvert.SerializeObject(quotation))
                    };
                    _db.HashSet($"{_coin}_quotation", hash);
                }
            }
            catch
            {
                // ignored
            }
        }

        private void WebSocket_Closed(object sender, EventArgs e)
        {
           //TODO:Log here...
        }

        private void WebSocket_Error(object sender, ErrorEventArgs e)
        {
            //TODO:Log here...
        }
    }
}