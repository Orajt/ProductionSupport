namespace Application.Core
{

    public static class Relations
    {
        public static List<ArticleTypeRelation> ArticleTypeRelations = new List<ArticleTypeRelation>
        {
            new ArticleTypeRelation(1,2),
            new ArticleTypeRelation(1,3),
            new ArticleTypeRelation(3,4),
            new ArticleTypeRelation(3,2),
            new ArticleTypeRelation(4,2),
        };
        public static List<ArticleTypeComponents> ArticleProperties = new List<ArticleTypeComponents>
        {
            new ArticleTypeComponents(1, true, false, true),
            new ArticleTypeComponents(2, false, true, false),
            new ArticleTypeComponents(3,false,false, false),
            new ArticleTypeComponents(4, false, true, false),
            new ArticleTypeComponents(5, true, false, false)
        };
        public static void FindEveryParentToArticleType(List<int> parents, int articleTypeId)
        {
            var newParents = ArticleTypeRelations.Where(p => p.Child == articleTypeId).Select(p => p.Parent).ToList();
            if (newParents.Count == 0)
                return;
            parents.AddRange(newParents);
            for (int i = 0; i < newParents.Count; i++)
            {
                FindEveryParentToArticleType(parents, newParents[i]);
            }
            parents = parents.Distinct().ToList();
        }

    }
}