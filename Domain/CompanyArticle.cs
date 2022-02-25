namespace Domain
{
    public class CompanyArticle
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public decimal Price { get; set; }
        public bool ForSale { get; set; }
        public string ClientName { get; set; }
    }
}