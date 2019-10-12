namespace QuotationDance.Collector
{
    public class CoinPair
    {
        public CoinPair()
        {

        }

        public CoinPair(string pair1, string pair2)
        {
            Pair1 = pair1;
            Pair2 = pair2;
        }

        public string Pair1 { get; set; }

        public string Pair2 { get; set; }
    }
}