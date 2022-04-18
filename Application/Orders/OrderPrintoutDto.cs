namespace Application.Orders
{
    public class OrderPrintoutDto
    {
        public int ArticleId { get; set; }
        public int PdfId { get; set; } = 0;
        public string ArticleName { get; set; }
        public int StuffId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        public string Stuff { get; set; }
        public int Quanity { get; set; }
        public List<string> ParentAndCount { get; set; } = new List<string>();
    }
}