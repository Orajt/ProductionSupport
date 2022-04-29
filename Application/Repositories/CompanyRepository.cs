using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class CompanyRepository : GenericRepository<Domain.Company>, ICompanyRepository
    {
        public CompanyRepository(DataContext context) : base(context)
        {

        }
    }
}