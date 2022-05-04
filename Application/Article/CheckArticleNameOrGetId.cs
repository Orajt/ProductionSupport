using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Article
{
    public class CheckArticleNameOrGetId
    {
        public class Query : IRequest<Result<int>>
        {
            public string ArticleName { get; set; }
            public string Predicate { get; set; }
            public int ArticleTypeId { get; set; }
            public int? StuffId { get; set; } = null;
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                int result = await _unitOfWork.Articles.IsArticleNameUsed
                (request.ArticleName, request.ArticleTypeId, request.StuffId) ? 1 : 0;

                return Result<int>.Success(result);
            }
        }
    }
}