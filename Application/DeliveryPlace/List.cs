using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DeliveryPlace
{
    public class List
    {
        public class Query : IRequest<Result<List<ListDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<ListDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<ListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var deliveryPlaces = await _context.DeliveryPlaces.Include(p=>p.Company)
                    .AsNoTracking()
                    .OrderBy(p=>p.Company.Name).ThenBy(p=>p.Name)
                    .ProjectTo<ListDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<ListDto>>.Success(deliveryPlaces);
            }
        }
    }
}