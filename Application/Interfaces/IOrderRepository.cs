namespace Application.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Domain.Order>
    {
         Task<Domain.Order> GetOrderWithArticleDetailsAndPositionRealizations(int id);
    }
}