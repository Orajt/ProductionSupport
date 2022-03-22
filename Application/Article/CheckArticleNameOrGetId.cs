using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class CheckArticleNameOrGetId
    {
        public class Query : IRequest<Result<int>>
        {
            public string ArticleName {get;set;}
            public string Predicate{get;set;}
            public int ArticleTypeId { get; set; }
            public int? StuffId{get;set;}=null;
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                int result=0;
                if(request.Predicate=="CHECKNAME")
                    result= await _context.Articles.AnyAsync(p=>p.ArticleTypeId==request.ArticleTypeId
                    && p.StuffId==(request.StuffId == 0 ? null : request.StuffId)
                    && p.FullName==request.ArticleName) ? 1 : 0;
                if(request.Predicate=="GETID")
                {
                    var article = await _context.Articles.FirstOrDefaultAsync(p=>p.ArticleTypeId==request.ArticleTypeId
                    && p.StuffId==request.StuffId 
                    && p.FullName==request.ArticleName);
                    if(article==null) return null;
                    result=article.Id;
                }
                return Result<int>.Success(result);
            }
        }
    }
}