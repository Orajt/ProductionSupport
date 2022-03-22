namespace Domain
{
    public class Stuff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArticleTypeId { get; set; }
        public ArticleType ArticleType { get; set; }
        public ICollection<Article> Articles { get; set; }=new List<Article>();
    }
}