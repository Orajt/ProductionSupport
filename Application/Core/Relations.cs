namespace Application.Core
{
    public interface IRelations
    {
        ArticleTypeComponents ArticleProperties(int id);
        List<ArticleTypeComponents> GetAllArticleProperties();
        List<ArticleTypeRelation> ArticleTypeRelations();
        void FindEveryParentToArticleType(List<int> parents, int articleTypeId);
    }

    public class Relations : IRelations
    {
        private List<ArticleTypeRelation> _articleTypeRelations { get; set; } = new List<ArticleTypeRelation>
        {
            new ArticleTypeRelation(1,2),
            new ArticleTypeRelation(1,3),
            new ArticleTypeRelation(3,4),
            new ArticleTypeRelation(3,2),
            new ArticleTypeRelation(4,2),
        };
        private List<ArticleTypeComponents> _articleProperties { get; set; } = new List<ArticleTypeComponents>
        {
            new ArticleTypeComponents(1, true, false, true),
            new ArticleTypeComponents(2, false, true, false),
            new ArticleTypeComponents(3,false,false, false),
            new ArticleTypeComponents(4, false, true, false),
            new ArticleTypeComponents(5, true, false, false)
        };
        public List<ArticleTypeComponents> GetAllArticleProperties()
        {
            return this._articleProperties;
        }
        public List<ArticleTypeRelation> ArticleTypeRelations()
        {
            return this._articleTypeRelations;
        }
        public ArticleTypeComponents ArticleProperties(int id)
        {
            return this._articleProperties.FirstOrDefault(p=>p.ArticleTypeId==id);
        }

        public void FindEveryParentToArticleType(List<int> parents, int articleTypeId)
        {
            var newParents = _articleTypeRelations.Where(p => p.Child == articleTypeId).Select(p => p.Parent).ToList();
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