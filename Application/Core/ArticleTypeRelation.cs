namespace Application.Core
{
    public class ArticleTypeRelation
    {
        public ArticleTypeRelation(int parentId, int childId)
        {
            this.Parent = parentId;
            this.Child = childId;
        }
        public int Parent;
        public int Child;
    }
}