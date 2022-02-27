namespace Application.ArticleArticle
{
    public class GetLowerLevelArticlesDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<LowerLevelArticle> LowerLevelArticles { get; set; } = new List<LowerLevelArticle>();
    }
    public class LowerLevelArticle
    {
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public string ArticleTypeName { get; set; }
        public string FamillyName { get; set; }
        public string StuffName { get; set; }
        public int Quanity { get; set; }
        public bool HasChild { get; set; }

    }
}