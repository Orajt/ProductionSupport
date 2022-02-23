namespace Domain
{
    public class ArticleArticle
    {
        public int ParentId { get; set; }
        public virtual Article ParentArticle { get; set; }
        public int ChildId { get; set; }
        public virtual Article ChildArticle { get; set; }
        public int PositionOnList { get; set; }
        public int Quanity { get; set; }
        public int AddCol { get; set; }
    }
}