namespace Application.Core
{

    public static class Relations
    {
        public static List<ArticleTypeRelation> ArticleTypeRelations=new List<ArticleTypeRelation>
        {
            new ArticleTypeRelation(1,2),
            new ArticleTypeRelation(1,3),
            new ArticleTypeRelation(3,4),
            new ArticleTypeRelation(3,2),
            new ArticleTypeRelation(4,2),
        };
    }
}