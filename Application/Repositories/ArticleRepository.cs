using Application.Article;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
    public class ArticleRepository : GenericRepository<Domain.Article>, IArticleRepository
    {
        private readonly IMapper _mapper;
        public ArticleRepository(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<ArticleAdditionalProperties> FindAdditionalProperties
        (int? famillyId = null, int? stuffId = null, int? fvgId = null)
        {
            var result = new ArticleAdditionalProperties();
            if (famillyId != null && famillyId != 0)
                result.Familly = await _context.Famillies.FirstOrDefaultAsync(p => p.Id == famillyId);

            if (stuffId != null && stuffId != 0)
                result.Stuff = await _context.Stuffs.FirstOrDefaultAsync(p => p.Id == stuffId);

            if (fvgId != null && fvgId != 0)
                result.FabricVariantGroup = await _context.FabricVariantGroups.FirstOrDefaultAsync(p => p.Id == fvgId);
            return result;
        }

        public async Task<Domain.Article> GetArticleWithChildRelationsById(int id)
        {
            return await _context.Articles.Include(p => p.ChildRelations).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsArticleNameUsed(string name, int articleTypeId, int? stuffId = null)
        {
            if (stuffId != 0)
                return await _context.Articles.AnyAsync(p => p.FullName.ToUpper() == name.ToUpper()
                                                 && p.ArticleTypeId == articleTypeId
                                                 && p.StuffId == stuffId);

            return await _context.Articles.AnyAsync(p => p.FullName.ToUpper() == name.ToUpper()
                                             && p.ArticleTypeId == articleTypeId);
        }
        new public void Add(Domain.Article entity)
        {
            _context.Set<Domain.Article>().Add(entity);
        }

        public async Task<Domain.Article> GetArticleWithChildAndParentRelations(int id)
        {
            return await _context.Articles
                .Include(p => p.ChildRelations)
                .Include(p => p.ParentRelations)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<DetailsDto> GetArticleDetailsBasedOnId(int id)
        {
            return await _context.Articles.ProjectTo<DetailsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<DetailsDto> GetArticleDetailsBasedOnName(string name)
        {
            var articles = await _context.Articles.Where(p => p.FullName.ToUpper()
                    == name.ToUpper()).ProjectTo<DetailsDto>(_mapper.ConfigurationProvider).ToListAsync();
            return articles.Last();
        }

        public IQueryable<ListDto> GetArticlesQueryMappedToListDto()
        {
            return _context.Articles
                    .OrderBy(p => p.FullName)
                    .ProjectTo<ListDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();
        }

        public async Task<List<ReactSelectInt>> GetArticlesPossibleToAssignToArticleType(int articleTypeId, List<int> possibleTypes)
        {
            var result = new List<ReactSelectInt>();
            var articles = await _context.Articles
                            .AsNoTracking()
                            .OrderBy(p => p.FullName)
                            .Include(p => p.Stuff)
                            .Include(p => p.Familly)
                            .Where(p => possibleTypes.Contains(p.ArticleTypeId))
                            .Select(p => new { p.Id, p.FullName, StuffName = p.Stuff.Name, FamillyName = p.Familly.Name })
                            .ToListAsync();
            foreach (var article in articles)
            {
                if (!String.IsNullOrEmpty(article.StuffName))
                {
                    result.Add(new ReactSelectInt() { Label = $"{article.FullName}({article.StuffName})", Value = article.Id });
                    continue;
                }
                if (!String.IsNullOrEmpty(article.FamillyName))
                {
                    result.Add(new ReactSelectInt() { Label = $"{article.FullName}({article.FamillyName})", Value = article.Id });
                    continue;
                }
                result.Add(new ReactSelectInt() { Label = $"{article.FullName}", Value = article.Id });
            }
            return result;
        }

        public async Task<List<ReactSelectInt>> GetAllArticlesBelongsToArticleType(int articleTypeId)
        {
            return await _context.Articles
                            .AsNoTracking()
                            .OrderBy(p => p.FullName)
                            .Where(p => p.ArticleTypeId == articleTypeId)
                            .Select(p => new ReactSelectInt { Label = p.FullName, Value = p.Id })
                            .ToListAsync();
        }

        public async Task<Domain.Article> GetArticleWithFabricVarGroupAndRealizations(int id)
        {
            return await _context.Articles
                    .Include(p => p.Realizations)
                        .ThenInclude(p => p.Stuff)
                    .Include(p => p.FabricVariant)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Domain.Article> GetArticleWithFabricVarGroupWithDetailsAndRealizations(int id)
        {
            return await _context.Articles
                    .Include(p => p.FabricVariant)
                        .ThenInclude(p => p.FabricVariants)
                    .Include(p => p.Realizations)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Domain.Article>> GetArticlesWithFabricVariantsBasedOnArtclesIds(List<int> articlesIds)
        {
            return await _context.Articles
                    .Include(p => p.FabricVariant)
                        .ThenInclude(p => p.FabricVariants)
                    .Where(p => articlesIds.Contains(p.Id))
                    .ToListAsync();
        }
    }
}