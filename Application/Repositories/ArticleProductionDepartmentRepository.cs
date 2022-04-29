using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class ArticleProductionDepartmentRepository : GenericRepository<Domain.ArticleProductionDepartment>, IArticleProductionDepartmentRepository
    {
        public ArticleProductionDepartmentRepository(DataContext context) : base(context)
        {

        }
    }
}