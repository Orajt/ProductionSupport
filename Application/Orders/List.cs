using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq.Dynamic.Core;

namespace Application.Orders
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
            private readonly IListHelpers _listHelpers;
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IListHelpers listHelpers, IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _listHelpers = listHelpers;

            }
            public async Task<Result<PagedList<ListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var queryString = _listHelpers.CreateQueryString(request.Filters);
                var query = _unitOfWork.Orders.GetOrdersQueryMappedToListDto();

                if (!String.IsNullOrEmpty(queryString))
                {
                    query = query.Where(queryString);
                }
                var result = await PagedList<ListDto>.CreateAsync(query, request.PagingParams.PageNumber,
                        request.PagingParams.PageSize);

                return Result<PagedList<ListDto>>.Success(result);
            }
        }
    }
}