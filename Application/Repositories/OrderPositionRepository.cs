using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
     public class OrderPositionRepository : GenericRepository<Domain.OrderPosition>, IOrderPositionRepository
    {
        public OrderPositionRepository(DataContext context) : base(context)
        {

        }

        public async Task<bool> AnyPositionsWithArticleId(int articleId)
        {
            return await _context.OrderPositions.AnyAsync(p=>p.ArticleId==articleId);
        }
    }
}