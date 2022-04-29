using System.Linq.Expressions;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
    public class StuffRepository : GenericRepository<Domain.Stuff>, IStuffRepository
    {
        public StuffRepository(DataContext context) : base(context)
        {
            
        }

        public async Task<bool> IsStuffNameUnique(string name)
        {
            return await _context.Stuffs.AnyAsync(p => p.Name.ToUpper() == name.ToUpper());
        }

    }
}