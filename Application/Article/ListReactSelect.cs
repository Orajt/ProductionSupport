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
                var articles = await _context.Articles
                    .AsNoTracking()
                    .Where(p=>p.ArticleTypeId==request.ArticleTypeId)
                    .Select(p=>new {p.Id, p.FullName})
                    .ToListAsync();
                
                var result = new List<ReactSelectInt>();

                foreach(var article in articles)
                {
                    result.Add(new ReactSelectInt{
                        Label=article.FullName,
                        Value=article.Id
                    });
                }

                return Result<List<ReactSelectInt>>.Success(result);
            }
        }
    }
}