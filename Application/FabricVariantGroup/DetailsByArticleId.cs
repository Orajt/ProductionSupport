using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariantGroup
{
    public class DetailsByArticleId
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public int Id { get; set; }
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
                var article= await _context.Articles
                    .Include(p=>p.FabricVariant)
                    .ThenInclude(p=>p.FabricVariants)
                    .ThenInclude(p=>p.FabricVariant)
                    .FirstOrDefaultAsync(p=>p.Id==request.Id);
              
              var fabricVariantGroup=article.FabricVariant;

                if(fabricVariantGroup==null) 
                    return Result<DetailsDto>.Success(new DetailsDto{Id=0});

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