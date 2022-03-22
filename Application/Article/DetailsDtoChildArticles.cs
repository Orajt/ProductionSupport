namespace Application.Article
{
    public class DetailsDtoChildArticles
    {
        public int ChildId { get; set; }
        public string ChildArticleName { get; set; }
        public string ChildArticleType { get; set; }
        public bool ChildArticleHasChild { get; set; }
        public int Quanity { get; set; }
    }
}