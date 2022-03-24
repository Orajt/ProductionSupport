namespace Domain
{
    public class Stuff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ArticleTypeStuff> ArticleTypes { get; set; }=new List<ArticleTypeStuff>();
        public ICollection<Article> Articles { get; set; }=new List<Article>();
    }
}