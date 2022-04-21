using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
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

                var stuffs= await _context.ArticleTypesStuffs
                    .Include(p=>p.Stuff)
                    .Where(p=>p.ArticleTypeId==request.ArticleTypeId)
                    .Select(p=> new ReactSelectInt{Label=p.Stuff.Name, Value=p.Stuff.Id})
                    .ToListAsync();
                
                return Result<List<ReactSelectInt>>.Success(stuffs);
            }
        }
    }
}