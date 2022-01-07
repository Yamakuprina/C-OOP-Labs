namespace Banks
{
    public class InterestRate
    {
        public InterestRate(int limit, decimal percent)
        {
            Limit = limit;
            Percent = percent;
        }

        public int Limit { get; set; }
        public decimal Percent { get; set; }
    }
}