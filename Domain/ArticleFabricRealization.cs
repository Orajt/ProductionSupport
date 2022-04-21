namespace Domain
{
    public class ArticleFabricRealization
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string CalculatedCode { get; set; }
        public int StuffId { get; set; }
        public Stuff Stuff { get; set; }
        public float FabricLength { get; set; }
    }
}