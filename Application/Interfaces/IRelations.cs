using Application.Core;

namespace Application.Interfaces
{
    public interface IRelations
    
    {
        ArticleTypeComponents ArticleProperties(int id);
        List<ArticleTypeComponents> GetAllArticleProperties();
        List<ArticleTypeRelation> ArticleTypeRelations();
        List<int> ArticleTypesIdsPossibleToAssign(int parentId);
        void FindEveryParentToArticleType(List<int> parents, int articleTypeId);
    }
    
}