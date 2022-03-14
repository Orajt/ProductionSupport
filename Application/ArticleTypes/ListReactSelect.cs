using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleTypes
{
    public class ListReactSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
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
                var articleTypes = await _context.ArticleTypes.Select(p=>new ReactSelectInt{Value=p.Id, Label=p.Name}).ToListAsync();

                return Result<List<ReactSelectInt>>.Success(articleTypes);
            }
        }
    }
}