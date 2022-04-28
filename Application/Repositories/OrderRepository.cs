using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class OrderRepository : GenericRepository<Domain.Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context)
        {

        }
    }
}