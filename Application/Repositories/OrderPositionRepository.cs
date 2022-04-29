using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class OrderPositionRepository : GenericRepository<Domain.OrderPosition>, IOrderPositionRepository
    {
        public OrderPositionRepository(DataContext context) : base(context)
        {

        }
    }
}