namespace Domain
{
    public class ArticleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; } = new List<Article>();
        public ICollection<ArticleTypeStuff> Stuffs { get; set; } = new List<ArticleTypeStuff>();
    }
}