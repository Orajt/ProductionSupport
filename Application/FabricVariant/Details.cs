using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariant
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
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<DetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                Domain.FabricVariant fabricVariant=null;
                var fabricVariantId=0;
                if(int.TryParse(request.Id, out fabricVariantId))
                    fabricVariant = await _context.FabricVariants.Include(p=>p.FabricVariantGroups).FirstOrDefaultAsync(p=>p.Id==fabricVariantId);
                if(fabricVariantId==0)
                    fabricVariant = await _context.FabricVariants.Include(p=>p.FabricVariantGroups).FirstOrDefaultAsync(p=>p.ShortName==request.Id.ToUpper());
                if(fabricVariant==null) return null;

                var result = new DetailsDto(){
                    Id=fabricVariant.Id,
                    ShortName=fabricVariant.ShortName,
                    FullName=fabricVariant.FullName,
                    AbleToDelete=!fabricVariant.FabricVariantGroups.Any()
                };

                return Result<DetailsDto>.Success(result);
            }
        }
    }
}