using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariantGroup
{
    public class ListReactSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
            public int FamillyId { get; set; }
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

                var fabricVarianGroupList = new List<Domain.FabricVariantGroup>();

                var result = new List<ReactSelectInt>();
                if (request.FamillyId != 0)
                {
                    fabricVarianGroupList = await _context.FamilliesFabricVarianGroups.Where(p=>p.FamillyId==request.FamillyId).Select(p=>p.FabricVariantGroup).ToListAsync();
                }
                if(request.FamillyId==0)
                    fabricVarianGroupList=await _context.FabricVariantGroups.ToListAsync();

                foreach (var variantGroup in fabricVarianGroupList)
                {
                    result.Add(new ReactSelectInt
                    {
                        Label = variantGroup.Name,
                        Value = variantGroup.Id
                    });
                }
                return Result<List<ReactSelectInt>>.Success(result);
            }
        }
    }
}