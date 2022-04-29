using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class ProductionDepartmentRepository : GenericRepository<Domain.ProductionDepartment>, IProductionDepartmentRepository
    {
        public ProductionDepartmentRepository(DataContext context) : base(context)
        {
            
        }
    }
}