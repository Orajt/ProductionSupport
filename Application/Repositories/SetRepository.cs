using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class SetRepository : GenericRepository<Domain.Set>, ISetRepository
    {
        public SetRepository(DataContext context) : base(context)
        {

        }
    }
}