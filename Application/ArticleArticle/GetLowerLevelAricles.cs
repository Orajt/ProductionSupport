using System.Diagnostics;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleArticle
{
    public class GetLowerLevelAricles
    {
        public class Query : IRequest<Result<GetLowerLevelArticlesDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<GetLowerLevelArticlesDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<GetLowerLevelArticlesDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var article = await _context.Articles.AsNoTracking()
                    .Include(p => p.ChildRelations)
                        .ThenInclude(p => p.ChildArticle)
                        .ThenInclude(p => p.ChildRelations)
                        .AsSplitQuery()
                    .Include(p=>p.ChildRelations)
                        .ThenInclude(p=>p.ChildArticle)
                        .ThenInclude(p=>p.ArticleType)
                    .Include(p=>p.ChildRelations)
                        .ThenInclude(p=>p.ChildArticle)
                        .ThenInclude(p=>p.Stuff)
                        .AsSplitQuery()
                    .Include(p=>p.ChildRelations)
                        .ThenInclude(p=>p.ChildArticle)
                        .ThenInclude(p=>p.Familly)
                        .AsSplitQuery()
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (article == null) return null;

                var result = new GetLowerLevelArticlesDto();
                result.Id = article.Id;
                result.FullName = article.FullName;
                var lowerLevelArticles = new List<LowerLevelArticle>();
                foreach (var child in article.ChildRelations)
                {
                    lowerLevelArticles.Add(new LowerLevelArticle
                    {
                        ArticleId=child.ChildId,
                        ArticleName=child.ChildArticle.FullName,
                        ArticleTypeName=child.ChildArticle.ArticleType.Name,
                        FamillyName=child.ChildArticle.Familly!=null ? child.ChildArticle.Familly.Name : "None",
                        StuffName=child.ChildArticle.Stuff!=null ? child.ChildArticle.Stuff.Name : "None",
                        Quanity=child.Quanity,
                        HasChild=child.ChildArticle.ChildRelations.Any()
                        
                    });   
                        
                }
                result.LowerLevelArticles=lowerLevelArticles;
                return Result<GetLowerLevelArticlesDto>.Success(result);
            }
        }
    }
}