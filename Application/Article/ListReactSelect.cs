using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class ListReactSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
            public int ArticleTypeId { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ReactSelectInt>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ReactSelectInt>>> Handle(Query request, CancellationToken cancellationToken)
            {
                if(request.ArticleTypeId!=0)
                {
                    var articleType = await _context.ArticleTypes.FirstOrDefaultAsync(p => p.Id == request.ArticleTypeId);
                    if (articleType == null) return null;
                }
               
                var articlesRS = new List<ReactSelectInt>();

                switch (request.Predicate)
                {
                    case "TOASSIGN":
                        var possibleTypes = Core.Relations.ArticleTypeRelations.Where(p => p.Parent == request.ArticleTypeId).Select(p => p.Child).ToList();
                        var articles = await _context.Articles
                            .AsNoTracking()
                            .Include(p=>p.Stuff)
                            .Include(p=>p.Familly)
                            .Where(p => possibleTypes.Contains(p.ArticleTypeId))
                            .Select(p => new{p.Id, p.FullName, StuffName=p.Stuff.Name, FamillyName=p.Familly.Name})
                            .ToListAsync();
                        foreach(var article in articles)
                        {
                            if(!String.IsNullOrEmpty(article.StuffName))
                            {
                                 articlesRS.Add(new ReactSelectInt(){Label=$"{article.FullName}({article.StuffName})", Value=article.Id});
                                 continue;
                            }
                            if(!String.IsNullOrEmpty(article.FamillyName))
                                articlesRS.Add(new ReactSelectInt(){Label=$"{article.FullName}({article.FamillyName})", Value=article.Id});

                        }
                        break;
                    case "FULLLIST":
                        articlesRS = await _context.Articles
                            .AsNoTracking()
                            .Where(p => p.ArticleTypeId == request.ArticleTypeId)
                            .Select(p => new ReactSelectInt { Label = p.FullName, Value = p.Id })
                            .ToListAsync();
                        break;
                    default:
                        return Result<List<ReactSelectInt>>.Failure("Choosen article type doesn't have any article assigned or predicate is wrong");
                }

                return Result<List<ReactSelectInt>>.Success(articlesRS);
            }
        }
    }
}