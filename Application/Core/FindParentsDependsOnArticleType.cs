using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Core
{
    public class FindParentsDependsOnArticleType
    {
        public bool ArticleOnHigherLevel = false;
        private List<int> PossibleArticles = new List<int>();
        private readonly DataContext _context;
        private readonly int _articleTypeId;
        private readonly List<int> _requestIds;
        public FindParentsDependsOnArticleType(List<int>requestIds, DataContext context, int articleTypeId)
        {
            _requestIds = requestIds;
            _articleTypeId = articleTypeId;
            _context = context;
        }
        public async Task IsArticleOnHigherLevel(List<int> ids)
        {

            if(ArticleOnHigherLevel)
                return;
             
            var newParents = await _context.ArticleArticle.AsNoTracking()
                .Include(p => p.ParentArticle).ThenInclude(p => p.ParentRelations)
                .Where(p => ids.Contains(p.ChildId) && p.ParentArticle.ArticleTypeId == _articleTypeId)
                .Select(p => p.ParentId)
                .ToListAsync();
            if (newParents.Count==0) return;

            this.PossibleArticles.AddRange(newParents);
            
            if (PossibleArticles.Any(p => _requestIds.Contains(p)))
                this.ArticleOnHigherLevel = true;
            await IsArticleOnHigherLevel(newParents);

           

        }


    }
}