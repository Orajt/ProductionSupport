using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class OrderPositionRealizationRepozitory : GenericRepository<Domain.OrderPositionRealization>, IOrderPositionRealizationRepozitory
    {
        public OrderPositionRealizationRepozitory(DataContext context) : base(context)
        {

        }
    }
}