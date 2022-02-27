using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class Details
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public int Id { get; set; }
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
                var order = await _context.Orders
                    .ProjectTo<DetailsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p=>p.Id==request.Id);

                return Result<DetailsDto>.Success(order);
            }
        }
    }
}