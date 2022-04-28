using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class FamillyRepository : GenericRepository<Domain.Familly>, IFamillyRepository
    {
        public FamillyRepository(DataContext context) : base(context)
        {

        }
    }
}