using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariant
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
                var fabricVariants = await _context.FabricVariants.ToListAsync();

                var result = new List<ReactSelectInt>();   
                foreach (var fabricVariant in fabricVariants)
                {
                    result.Add(new ReactSelectInt
                    {
                        Label = $"{fabricVariant.FullName}({fabricVariant.ShortName})",
                        Value = fabricVariant.Id
                    });
                }
                result=result.OrderBy(p=>p.Label).ToList();
                return Result<List<ReactSelectInt>>.Success(result);
            }
        }
    }
}