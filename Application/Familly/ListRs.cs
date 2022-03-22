using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Familly
{
    public class ListRs
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

                var familliesRS = await _context.Famillies
                    .AsNoTracking()
                    .Select(p => new ReactSelectInt { Label = p.Name, Value = p.Id })
                    .ToListAsync();

                return Result<List<ReactSelectInt>>.Success(familliesRS);
            }
        }
    }
}