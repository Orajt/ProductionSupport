using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class Details
    {
        public class Query : IRequest<DetailsDto>
        {
            public int ArticleId{get;set;}
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
                var article = await _context.Articles.ProjectTo<DetailsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(p=>p.Id==request.ArticleId);
                
                return Result<DetailsDto>.Success(_mapper.Map<DetailsDto>(article));
            }
        }
    }
}