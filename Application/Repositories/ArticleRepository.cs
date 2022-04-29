using Application.Article;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
    public class ArticleRepository : GenericRepository<Domain.Article>, IArticleRepository
    {
        public ArticleRepository(DataContext context) : base(context)
        {
        }
        public async Task<ArticleAdditionalProperties> FindAdditionalProperties
        (int? famillyId = null, int? stuffId = null, int? fvgId = null)
        {
            var result = new ArticleAdditionalProperties();
            if(famillyId !=null && famillyId!=0)
                result.Familly = await _context.Famillies.FirstOrDefaultAsync(p => p.Id == famillyId);

            if(stuffId !=null && stuffId!=0)
                result.Stuff = await _context.Stuffs.FirstOrDefaultAsync(p => p.Id == stuffId);

            if(fvgId !=null && fvgId!=0)
                result.FabricVariantGroup = await _context.FabricVariantGroups.FirstOrDefaultAsync(p => p.Id == fvgId);
            return result;
        }

        public async Task<Domain.Article> GetArticleWithChildRelationsById(int id)
        {
            return await _context.Articles.Include(p => p.ChildRelations).FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<bool> IsArticleNameUnique(string name, int articleTypeId, int? stuffId=null)
        {
            return await _context.Articles.AnyAsync(p => p.FullName.ToUpper() == name.ToUpper()
                                             && p.ArticleTypeId == articleTypeId
                                             && p.StuffId == stuffId);
            
        }
        new public void Add(Domain.Article entity)
        {
            _context.Set<Domain.Article>().Add(entity);
        }

    }
}