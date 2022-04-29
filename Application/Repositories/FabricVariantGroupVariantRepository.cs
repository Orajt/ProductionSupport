using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class FabricVariantGroupVariantRepository : GenericRepository<Domain.FabricVariantFabricGroupVariant>, IFabricVariantGroupVariantRepository
    {
        public FabricVariantGroupVariantRepository(DataContext context) : base(context)
        {

        }
    }
}