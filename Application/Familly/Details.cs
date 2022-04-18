using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Familly
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
                Domain.Familly familly=null;
                var famillyId=0;
                if(int.TryParse(request.Id, out famillyId))
                    familly = await _context.Famillies
                        .Include(p=>p.Articles)
                        .Include(p=>p.FabricVariantGroups)
                            .ThenInclude(p=>p.FabricVariantGroup)
                        .FirstOrDefaultAsync(p=>p.Id==famillyId);
                if(famillyId==0)
                     familly = await _context.Famillies
                        .Include(p=>p.Articles)
                        .Include(p=>p.FabricVariantGroups)
                            .ThenInclude(p=>p.FabricVariantGroup)
                        .FirstOrDefaultAsync(p=>p.Name.ToUpper()==request.Id.ToUpper());

                if(familly==null) return null;

                var result = new DetailsDto(){
                    Id=familly.Id,
                    Name=familly.Name,
                };

                foreach(var article in familly.Articles)
                {
                    result.Articles.Add(new ReactSelectInt(){
                        Label=article.FullName,
                        Value=article.Id
                    });
                }
                foreach(var fVG in familly.FabricVariantGroups)
                {
                    result.Articles.Add(new ReactSelectInt(){
                        Label=fVG.FabricVariantGroup.Name,
                        Value=fVG.FabricVariantGroupId
                    });
                }
                result.Articles=result.Articles.OrderBy(p=>p.Label).ToList();
                result.FabricGroupVariants=result.FabricGroupVariants.OrderBy(p=>p.Label).ToList();

                if(!result.Articles.Any())
                    result.AbleToDelete=true;

                return Result<DetailsDto>.Success(result);
            }
        }
    }
}