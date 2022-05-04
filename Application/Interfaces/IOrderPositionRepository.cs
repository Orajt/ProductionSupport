namespace Application.Interfaces
{
    public interface IOrderPositionRepository : IGenericRepository<Domain.OrderPosition>
    {
           Task<bool> AnyPositionsWithArticleId(int articleId);
    }
}