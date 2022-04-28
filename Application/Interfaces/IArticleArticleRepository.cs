namespace Application.Interfaces
{
    public interface IArticleArticleRepository : IGenericRepository<Domain.ArticleArticle>
    {
        Task<List<Domain.ArticleArticle>> GetComponentsToParentAricle(List<Article.DetailsDtoChildArticles> components, Domain.Article parent);
    }
}