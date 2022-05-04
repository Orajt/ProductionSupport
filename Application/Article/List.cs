using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq.Dynamic.Core;

namespace Application.Article
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
            private readonly IUnitOfWork _unitOfWork;
            private readonly IListHelpers _listHelpers;
            public Handler(IUnitOfWork unitOfWork, IListHelpers listHelpers)
            {
                _listHelpers = listHelpers;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<PagedList<ListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryString = _listHelpers.CreateQueryString(request.Filters);

                var articlesQuery = _unitOfWork.Articles.GetArticlesQueryMappedToListDto();

                if (!String.IsNullOrEmpty(queryString))
                {
                    articlesQuery = articlesQuery.Where(queryString);
                }
                var result = await PagedList<ListDto>.CreateAsync(articlesQuery, request.PagingParams.PageNumber,
                        request.PagingParams.PageSize);

                return Result<PagedList<ListDto>>.Success(result);
            }
        }
    }
}