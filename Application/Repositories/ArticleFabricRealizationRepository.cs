using Application.Interfaces;
using Persistence;

namespace Application.Repositories
{
    public class ArticleFabicRealizationRepository : GenericRepository<Domain.ArticleFabricRealization>, IArticleFabricRealizationRepository
    {
        public ArticleFabicRealizationRepository(DataContext context) : base(context)
        {

        }
    }
}