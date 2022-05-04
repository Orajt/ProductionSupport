using static Application.Orders.OrdersHelper;

namespace Application.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Domain.Order>
    {
         Task<Domain.Order> GetOrderWithArticleDetailsAndPositionRealizations(int id);
         IQueryable<Orders.ListDto> GetOrdersQueryMappedToListDto();

         Task<Domain.Order> GetOrderWithOrderPositions(int id);
         Task<bool> IsOrderNameTaken(string orderName);

    }
}