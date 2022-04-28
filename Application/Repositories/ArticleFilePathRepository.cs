using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
     public class ArticleFilePathRepository : GenericRepository<Domain.ArticleFilePath>, IArticleFilePathRepository
    {
        public ArticleFilePathRepository(DataContext context) : base(context)
        {

        }
    }
}