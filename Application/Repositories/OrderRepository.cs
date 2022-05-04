using Application.Interfaces;
using Application.Orders;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
    public class OrderRepository : GenericRepository<Domain.Order>, IOrderRepository
    {
        private readonly IMapper _mapper;
        public OrderRepository(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;

        }

        public IQueryable<ListDto> GetOrdersQueryMappedToListDto()
        {
            return _context.Orders.ProjectTo<ListDto>(_mapper.ConfigurationProvider).AsQueryable();
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

        public async Task<Order> GetOrderWithOrderPositions(int id)
        {
            return await _context.Orders.Include(p => p.OrderPositions)
                .ThenInclude(p => p.Set)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsOrderNameTaken(string orderName)
        {
            return await _context.Orders.AsNoTracking()
                    .AnyAsync(p => p.Name.ToUpper() == orderName.ToUpper());
        }
    }
}