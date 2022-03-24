using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
{
    public class ListToSelect
    {
        public class Query : IRequest<Result<List<ListToSelectDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<ListToSelectDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ListToSelectDto>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var stuffs = await _context.ArticleTypesStuffs
                    .Include(p=>p.Stuff)
                    .AsNoTracking()
                    .Select(p => new ListToSelectDto{Label=p.Stuff.Name, Value=p.StuffId, ArticleTypeId=p.ArticleTypeId})
                    .ToListAsync();
                
                return Result<List<ListToSelectDto>>.Success(stuffs);
            }
        }
    }
}