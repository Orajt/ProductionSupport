using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Companies
{
    public class List
    {
        public class Query : IRequest<Result<List<ListDto>>>
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string CompanyIdentifier { get; set; }
            public bool? Seller { get; set; }
            public bool? Buyier { get; set; }
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
                var companies = await _context.Companies.ProjectTo<ListDto>(_mapper.ConfigurationProvider).ToListAsync();

                return Result<List<ListDto>>.Success(companies);
            }
        }

    }
}