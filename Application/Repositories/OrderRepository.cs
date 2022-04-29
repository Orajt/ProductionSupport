using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
     public class OrderRepository : GenericRepository<Domain.Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context)
        {

        }

        public async Task<Order> GetOrderWithArticleDetailsAndPositionRealizations(int id)
        {
            return await _context.Orders
                    .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Realizations)
                            .ThenInclude(p => p.Variant)
                     .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Realizations)
                            .ThenInclude(p => p.Fabric)
                                .ThenInclude(p => p.Stuff)
                    .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Article)
                            .ThenInclude(p => p.FabricVariant)
                                .ThenInclude(p => p.FabricVariants)
                    .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Article)
                             .ThenInclude(p => p.Realizations)
                        .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}