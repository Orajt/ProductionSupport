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
        public static List<ArticleTypeComponents> ArticleTypeComponents = new List<ArticleTypeComponents>
        {
            new ArticleTypeComponents(1, true, false),
            new ArticleTypeComponents(2, false, true),
            new ArticleTypeComponents(3, true, true),
            new ArticleTypeComponents(4, false, true),
        };
        
    }
}