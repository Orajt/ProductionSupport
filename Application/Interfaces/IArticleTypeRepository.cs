namespace Application.Interfaces
{
    public interface IArticleTypeRepository : IGenericRepository<Domain.ArticleType>
    {
         Task<Domain.ArticleType> GetArticleTypeWithStuffs(int id);
    }
}