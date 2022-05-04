using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
     public class ArticleTypeRepository : GenericRepository<Domain.ArticleType>, IArticleTypeRepository
    {
        public ArticleTypeRepository(DataContext context) : base(context)
        {
            
        }

        public async Task<ArticleType> GetArticleTypeWithStuffs(int id)
        {
            return await _context.ArticleTypes
                    .Include(p => p.Stuffs)
                    .ThenInclude(p => p.Stuff)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}