namespace QuotationDance.Collector
{
    public class TempConfig
    {
        public static WebSocketConfig Huobi => new WebSocketConfig
        {
            Url = "wss://api.huobi.br.com/ws",
            Request = new WebSocketRequest
            {
                Pattern = "{\"sub\":\"market.{pair1}{pair2}.detail\",\"symbol\":\"{pair1}{pair2}\"}",
                PairFormat = RequestPairFormat.Lower
            },
            Proxy = new WebSocketProxy
            {
                Host = "127.0.0.1",
                Port = 1080
            },
            Response = new WebSocketResponse
            {
                Type = "gzip",
                ParseType = "json",
                JsonParseConfig = new JsonParseConfig
                {
                    TimeJPath = "$.ts",
                    TimeParseFun = "ParseUnixMilliseconds",
                    PriceJPath = "$.tick.close",
                    VolumeJPath = "$.tick.amount"
                }
            }
        };

        public static WebSocketConfig Okex => new WebSocketConfig
        {
            Url = "wss://okexcomreal.bafang.com:8443/ws/v3",
            Request = new WebSocketRequest
            {
                Pattern = "{\"op\":\"subscribe\",\"args\":[\"spot/ticker:{pair1}-{pair2}\"]}",
                PairFormat = RequestPairFormat.Upper
            },
            Proxy = new WebSocketProxy
            {
                Host = "127.0.0.1",
                Port = 1080
            },
            Response = new WebSocketResponse
            {
                Type = "deflate",
                ParseType = "json",
                JsonParseConfig = new JsonParseConfig
                {
                    TimeJPath = "$.data[0].timestamp",
                    TimeParseFun = "ParseUtc",
                    PriceJPath = "$.data[0].last",
                    VolumeJPath = "$.data[0].base_volume_24h"
                }
            }
        };

        public static WebSocketConfig CoinBase => new WebSocketConfig
        {
            Url = "wss://ws-feed.pro.coinbase.com/",
            Request = new WebSocketRequest
            {
                Pattern = "{\"type\":\"subscribe\",\"channels\":[{\"name\":\"ticker\",\"product_ids\":[\"{pair1}-{pair2}\"]}]}",
                PairFormat = RequestPairFormat.Upper
            },
            Proxy = new WebSocketProxy
            {
                Host = "127.0.0.1",
                Port = 1080
            },
            Response = new WebSocketResponse
            {
                Type = "string",
                ParseType = "json",
                JsonParseConfig = new JsonParseConfig
                {
                    TimeJPath = "$.time",
                    TimeParseFun = "ParseUtc",
                    PriceJPath = "$.price",
                    VolumeJPath = "$.volume_24h"
                }
            }
        };
    }
}