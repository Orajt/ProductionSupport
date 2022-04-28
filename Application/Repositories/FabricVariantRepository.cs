using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class FabricVariantRepository : GenericRepository<Domain.FabricVariant>, IFabricVariantRepository
    {
        public FabricVariantRepository(DataContext context) : base(context)
        {

        }
    }
}