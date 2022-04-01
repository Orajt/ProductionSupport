using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleTypes
{
    public class Details
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public int ArticleTypeId { get; set; }
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
                var articleType = await _context.ArticleTypes
                    .Include(p => p.Stuffs)
                    .ThenInclude(p => p.Stuff)
                    .FirstOrDefaultAsync(p => p.Id == request.ArticleTypeId);

                if (articleType == null) return null;

                var stuffs = new List<ReactSelectInt>();

                if (articleType.Stuffs.Count > 0)
                {
                    foreach(var stuff in articleType.Stuffs)
                    {
                        stuffs.Add(new ReactSelectInt{
                            Label=stuff.Stuff.Name,
                            Value=stuff.StuffId
                        });
                    }
                }

                var result = new DetailsDto
                {
                    Id = articleType.Id,
                    Name = articleType.Name,
                    Stuffs=stuffs
                };

                return Result<DetailsDto>.Success(result);
            }
        }
    }
}