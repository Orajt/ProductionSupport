using Application.Article;

namespace Application.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Domain.Article>
    {
        Task<bool> IsArticleNameUsed(string name, int articleTypeId, int? stuffId);
        Task<ArticleAdditionalProperties> FindAdditionalProperties(int? famillyId = null, int? stuffId = null, int? fvgId = null);
        Task<Domain.Article> GetArticleWithChildRelationsById(int id);
        Task<Domain.Article> GetArticleWithChildAndParentRelations(int id);
        new void Add(Domain.Article entity);
        Task<Article.DetailsDto> GetArticleDetailsBasedOnId(int id);
        Task<Article.DetailsDto> GetArticleDetailsBasedOnName(string name);
        IQueryable<ListDto> GetArticlesQueryMappedToListDto();
        Task<List<ReactSelectInt>> GetArticlesPossibleToAssignToArticleType(int articleTypeId, List<int> possibleTypes);
        Task<List<ReactSelectInt>> GetAllArticlesBelongsToArticleType(int articleTypeId);

        Task<Domain.Article> GetArticleWithFabricVarGroupAndRealizations(int id);
        Task<Domain.Article> GetArticleWithFabricVarGroupWithDetailsAndRealizations(int id);
        Task<List<Domain.Article>> GetArticlesWithFabricVariantsBasedOnArtclesIds(List<int> articleIds);

    }
}