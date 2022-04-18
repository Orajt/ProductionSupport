using Application.Core;
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
            public PagingParams PagingParams{get;set;}
            public List<FilterResult> Filters{get;set;}
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ListDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<PagedList<ListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryString = ListHelpers.CreateQueryString(request.Filters);
                var articlesQuery = _context.Articles
                    .OrderBy(p=>p.FullName)
                    .ProjectTo<ListDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();
                
                if(!String.IsNullOrEmpty(queryString)){
                    articlesQuery=articlesQuery.Where(queryString);
                }
                var result=await PagedList<ListDto>.CreateAsync(articlesQuery, request.PagingParams.PageNumber,
                        request.PagingParams.PageSize);

                // foreach(var el in result)
                // {
                //     el.EditDate=DateHelpers.SetDateTimeToCurrent(el.EditDate);
                //     el.CreateDate=DateHelpers.SetDateTimeToCurrent(el.CreateDate);
                // }
                return Result<PagedList<ListDto>>.Success(result);
            }
        }
    }
}