using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class ArticleTypeRepository : GenericRepository<Domain.ArticleType>, IArticleTypeRepository
    {
        public ArticleTypeRepository(DataContext context) : base(context)
        {

        }
    }
}