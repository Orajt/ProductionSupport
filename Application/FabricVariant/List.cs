using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariant
{
    public class List
    {
        public class Query : IRequest<Result<List<DetailsDto>>>
        {
            
        }

        public class Handler : IRequestHandler<Query, Result<List<DetailsDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<DetailsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var fabricVariants = await _context.FabricVariants.Include(p=>p.FabricVariantGroups)
                    .Select(p=>new DetailsDto{FullName=p.FullName, ShortName=p.ShortName, Id=p.Id}).ToListAsync();

                return Result<List<DetailsDto>>.Success(fabricVariants);
            }
        }
    }
}