using Application.Core;
using Application.TreeHelpers;
using MediatR;
using Persistence;

namespace Application.Calculations
{
    public class CalculateArticlesByArticleType
    {
        public class Query : IRequest<Result<List<CalculateArticlesBasedOnArticleTypeResult>>>
        {
            public int OrderId { get; set; }
            public int ArticleTypeId{get;set;}
        }

        public class Handler : IRequestHandler<Query, Result<List<CalculateArticlesBasedOnArticleTypeResult>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<CalculateArticlesBasedOnArticleTypeResult>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var articleType = Relations.ArticleTypeComponents.FirstOrDefault(p=>p.ArticleTypeId==request.ArticleTypeId);
                return Result<List<CalculateArticlesBasedOnArticleTypeResult>>.Success(await CalculateArticlesBasedOnArticleTypeId.CalculateArticles(0,1,request.ArticleTypeId,new List<CalculateArticlesBasedOnArticleTypeResult>(),_context, articleType.HasFamilly, articleType.HasStuff,true,request.OrderId));
            }
        }
    }
}