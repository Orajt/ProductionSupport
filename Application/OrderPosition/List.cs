using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq.Dynamic.Core;

namespace Application.OrderPosition
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ListDto>>>
        {
            public PagingParams PagingParams { get; set; }
            public List<FilterResult> Filters { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<PagedList<ListDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IListHelpers _listHelpers;
            public Handler(DataContext context, IMapper mapper, IListHelpers listHelpers)
            {
                _listHelpers = listHelpers;
                _mapper = mapper;
                _context = context;
            }
            public async Task<Result<PagedList<ListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var queryString = _listHelpers.CreateQueryString(request.Filters);
                var query = _context.OrderPositions.ProjectTo<ListDto>(_mapper.ConfigurationProvider).AsQueryable();

                if (!String.IsNullOrEmpty(queryString))
                {
                    query = query.Where(queryString);
                }

                var result = await PagedList<ListDto>.CreateAsync(query, request.PagingParams.PageNumber,
                        request.PagingParams.PageSize);

                foreach (var el in result)
                {
                    el.ProductionDate = DateHelpers.SetDateTimeToCurrent(el.ProductionDate);
                    el.ShipmentDate = DateHelpers.SetDateTimeToCurrent(el.ShipmentDate);
                }
                return Result<PagedList<ListDto>>.Success(result);
            }
        }
    }
}