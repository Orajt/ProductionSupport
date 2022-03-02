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
                var articleType = await _context.ArticleTypes.FirstOrDefaultAsync(p => p.Id == request.ArticleTypeId);
                if (articleType == null) return null;
                var articlesRS = new List<ReactSelectInt>();

                switch (request.Predicate)
                {
                    case "TO ASSIGN":
                        var possibleTypes = Core.Relations.ArticleTypeRelations.Where(p => p.Parent == request.ArticleTypeId).Select(p => p.Child).ToList();
                        articlesRS = await _context.Articles
                            .AsNoTracking()
                            .Where(p => possibleTypes.Contains(p.ArticleTypeId))
                            .Select(p => new ReactSelectInt { Label = p.FullName, Value = p.Id })
                            .ToListAsync();
                        break;

                    case "FULL LIST":
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