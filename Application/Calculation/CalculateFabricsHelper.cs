namespace Application.Calculation
{
    public class CalculateFabricsHelper
    {

    }
    public class CalculateFabricsFabrics
    {
        public int FabricId { get; set; }
        public string FabricName { get; set; }
        public string Company { get; set; }
        public float Quanity { get; set; }

    }
    public class CalculateFabricsPosition
    {
        public long OrderPositionId { get; set; }
        public string Article { get; set; }
        public string CalculatedRealization { get; set; }
        public bool FabricsCalculated { get; set; }
        public int Quanity { get; set; }
        public string Client { get; set; }
    }
    public class CalculateFabricsUnableToFind
    {
        public long PositionId { get; set; }
        public string ArticleName { get; set; }
        public string Code { get; set; }
        public string StuffName { get; set; }
    }
    public class CodeStuffResult
    {
        public string Code { get; set; } = "";
        public string StuffName = "";
        public string Variants = "";
        public int StuffId { get; set; } = 0;
        public string FabricName { get; set; } = "";
        public int FabricId { get; set; } = 0;
        public float FabricLength { get; set; } = 0;

    }
}