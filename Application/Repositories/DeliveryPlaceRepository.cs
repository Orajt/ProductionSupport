using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class DeliveryPlaceRepository : GenericRepository<Domain.DeliveryPlace>, IDeliveryPlaceRepository
    {
        public DeliveryPlaceRepository(DataContext context) : base(context)
        {

        }
    }
}