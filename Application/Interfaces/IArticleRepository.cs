using Application.Article;

namespace Application.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Domain.Article>
    {
        Task<bool> IsArticleNameUnique(string name, int articleTypeId, int? stuffId);
        Task<ArticleAdditionalProperties> FindAdditionalProperties(int? famillyId = null, int? stuffId = null, int? fvgId = null);
        Task<Domain.Article> GetArticleWithChildRelationsById(int id);
        new void Add(Domain.Article entity);
    }
}