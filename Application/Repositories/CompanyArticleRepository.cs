using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class CompanyArticleRepository : GenericRepository<Domain.CompanyArticle>, ICompanyArticleRepository
    {
        public CompanyArticleRepository(DataContext context) : base(context)
        {

        }
    }
}