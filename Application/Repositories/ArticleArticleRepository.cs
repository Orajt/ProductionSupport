using Application.Article;
using Application.Core;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
    public class ArticleArticleRepository : GenericRepository<Domain.ArticleArticle>, IArticleArticleRepository
    {
        public ArticleArticleRepository(DataContext context) : base(context)
        {

        }


        public async Task<List<Domain.ArticleArticle>> GetComponentsToParentAricle(List<DetailsDtoChildArticles> components, Domain.Article parent)
        {
            var possibleChildTypes = Relations.ArticleTypeRelations.Where(p => p.Parent == parent.ArticleTypeId).Select(p => p.Child).ToList();
            var articlesToAssignFromDatabase = await _context.Articles.Where(p => components.Select(p=>p.ChildId).Contains(p.Id)).ToListAsync();
            var result=new List<Domain.ArticleArticle>();
            foreach (var component in components)
            {
                var componentDB = articlesToAssignFromDatabase.FirstOrDefault(p => p.Id == component.ChildId);
                if (componentDB == null) return null;
                if (!possibleChildTypes.Any(p => p == componentDB.ArticleTypeId))
                   return new List<Domain.ArticleArticle>();
                result.Add(new Domain.ArticleArticle { ParentArticle = parent, ParentId = parent.Id, ChildArticle = componentDB, ChildId = componentDB.Id, Quanity = component.Quanity });
            }
            return result;
        }
    }
}