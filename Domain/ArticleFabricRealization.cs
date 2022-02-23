namespace Domain
{
    public class ArticleFabricRealization
    {
        public long Id { get; set; }
        public int ArticleId { get; set; } 
        public Article Article { get; set; }
        public int CalculatedCode { get; set; }
        public int StuffId { get; set; }
        public int FabricVariantId { get; set; }
        public int GroupId { get; set; }
        public float FabricLength { get; set; }
    }
}