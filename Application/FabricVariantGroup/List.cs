using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariantGroup
{
    public class List
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
                var fabricVarianGroupList = await _context.FabricVariantGroups.OrderBy(p=>p.Name).Select(p=>new ReactSelectInt{Value=p.Id, Label=p.Name}).ToListAsync();

                return Result<List<ReactSelectInt>>.Success(fabricVarianGroupList);
            }
        }
    }
}