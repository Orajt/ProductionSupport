using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class ArticleTypeStuffRepository : GenericRepository<Domain.ArticleTypeStuff>, IArticleTypeStuffRepository
    {
        public ArticleTypeStuffRepository(DataContext context) : base(context)
        {

        }
    }
}