using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class FabricVariantGroupRepository : GenericRepository<Domain.FabricVariantGroup>, IFabricVariantGroupRepository
    {
        public FabricVariantGroupRepository(DataContext context) : base(context)
        {

        }
    }
}