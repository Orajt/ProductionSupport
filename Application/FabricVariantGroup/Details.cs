using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariantGroup
{
    public class Details
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<DetailsDto>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<DetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                Domain.FabricVariantGroup fabricVariantGroup=null;
                var fabricVariantGroupId=0;
                if(int.TryParse(request.Id, out fabricVariantGroupId))
                    fabricVariantGroup = await _context.FabricVariantGroups.Include(p=>p.FabricVariants).ThenInclude(p=>p.FabricVariant).FirstOrDefaultAsync(p=>p.Id==fabricVariantGroupId);
                if(fabricVariantGroupId==0)
                     fabricVariantGroup = await _context.FabricVariantGroups.Include(p=>p.FabricVariants).ThenInclude(p=>p.FabricVariant).FirstOrDefaultAsync(p=>p.Name==request.Id);

                if(fabricVariantGroup==null) return null;

                var result = new DetailsDto(){
                    Id=fabricVariantGroup.Id,
                    Name=fabricVariantGroup.Name,

                };
                foreach(var fabricVariant in fabricVariantGroup.FabricVariants)
                {
                    result.FabricVariants.Add(new DetailsDtoFabricVariant{
                        Id=fabricVariant.FabricVariantId,
                        FullName=fabricVariant.FabricVariant.FullName,
                        ShortName=fabricVariant.FabricVariant.ShortName,
                        PlaceInGroup=fabricVariant.PlaceInGroup
                    });
                }
                result.FabricVariants=result.FabricVariants.OrderBy(p=>p.PlaceInGroup).ToList();

                return Result<DetailsDto>.Success(result);
            }
        }
    }
}