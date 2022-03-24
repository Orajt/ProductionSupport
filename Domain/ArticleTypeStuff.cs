namespace Domain
{
    public class ArticleTypeStuff
    {
        public int ArticleTypeId { get; set; }
        public ArticleType ArticleType { get; set; }
        public int StuffId { get; set; }
        public Stuff Stuff{get;set;}
    }
}