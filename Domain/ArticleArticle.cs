namespace Domain
{
    public class ArticleArticle
    {
        public ArticleArticle()
        {
        }
        public ArticleArticle(Article parent, Article child, int quanity, int position)
        {
            ParentId=parent.Id;
            ParentArticle=parent;
            ChildId=child.Id;
            ChildArticle=child;
            Quanity=quanity;
        }
        public int ParentId { get; set; }
        public virtual Article ParentArticle { get; set; }
        public int ChildId { get; set; }
        public virtual Article ChildArticle { get; set; }
        public int Quanity { get; set; }
        public int AddCol { get; set; }

    }
}