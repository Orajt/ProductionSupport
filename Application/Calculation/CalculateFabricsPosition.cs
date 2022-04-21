namespace Application.Calculation
{
    public class CalculateFabricsPosition
    {
        public long OrderPositionId { get; set; }
        public string Article { get; set; }
        public string CalculatedRealization { get; set; }
        public bool FabricsCalculated { get; set; }
        public int Quanity { get; set; }
        public string Client { get; set; }
    }
}