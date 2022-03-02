namespace Application.TreeHelpers
{
    public class CalculateArticlesBasedOnArticleTypeResult
    {
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public int? ParentArticleId { get; set; }
        public string ParentArticleName { get; set; }
        public int? StuffId { get; set; }
        public string StuffName { get; set; }
        public int? FamillyId { get; set; }
        public string FamillyName { get; set; }
        public int Quanity { get; set; }
    }
}