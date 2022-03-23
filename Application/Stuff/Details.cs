using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
{
    public class Details
    {
        public class Query : IRequest<Result<ListDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ListDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<ListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var stuff = await _context.Stuffs
                    .AsNoTracking()
                    .ProjectTo<ListDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p=>p.Id==request.Id);

                return Result<ListDto>.Success(stuff);
            }
        }
    }
}