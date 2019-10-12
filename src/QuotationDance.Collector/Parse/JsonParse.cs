using Newtonsoft.Json.Linq;

namespace QuotationDance.Collector.Parse
{
    public class JsonParse
    {
        public Quotation Parse(string content, JsonParseConfig config)
        {
            var o = JObject.Parse(content);
            var time = o.SelectToken(config.TimeJPath).ToString();
            var quotation = new Quotation
            {
                Time = TimeParse.Parse(config.TimeParseFun, time),
                Price = double.Parse(o.SelectToken(config.PriceJPath).ToString()),
                Volume = double.Parse(o.SelectToken(config.VolumeJPath).ToString()),
            };

            return quotation;
        }
    }
}