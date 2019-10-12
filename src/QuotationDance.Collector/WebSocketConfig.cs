namespace QuotationDance.Collector
{
    public class WebSocketConfig
    {
        public string Url { get; set; }

        public WebSocketRequest Request { get; set; }

        public WebSocketProxy Proxy { get; set; }

        public WebSocketResponse Response { get; set; }
    }

    public class WebSocketRequest
    {
        public string Pattern { get; set; }

        public RequestPairFormat PairFormat { get; set; }
    }

    public class WebSocketResponse
    {
        public string Type { get; set; }

        public string ParseType { get; set; }

        public JsonParseConfig JsonParseConfig { get; set; }
    }

    public class JsonParseConfig
    {
        public string TimeJPath { get; set; }

        public string TimeParseFun { get; set; }

        public string PriceJPath { get; set; }

        public string VolumeJPath { get; set; }
    }

    public class WebSocketProxy
    {
        public string Host { get; set; }

        public int Port { get; set; }
    }

    public enum RequestPairFormat
    {
        Normal,
        Lower,
        Upper
    }
}