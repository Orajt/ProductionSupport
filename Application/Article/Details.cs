using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Article
{
    public class Details
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public string ArticleIdentifier { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<DetailsDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<DetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var article = new DetailsDto();
                int articleId = 0;
                if (int.TryParse(request.ArticleIdentifier, out articleId))
                {
                    article = await _unitOfWork.Articles.GetArticleDetailsBasedOnId(articleId);
                }
                if (articleId == 0)
                {
                    article = await _unitOfWork.Articles.GetArticleDetailsBasedOnName(request.ArticleIdentifier);
                }
                if (article == null) return null;

                article.EditDate = DateHelpers.SetDateTimeToCurrent(article.EditDate);
                article.CreateDate = DateHelpers.SetDateTimeToCurrent(article.CreateDate);

                article.AbleToEditPrimaries = !(await _unitOfWork.OrderPositions.AnyPositionsWithArticleId(article.Id));
                article.ChildArticles = article.ChildArticles.OrderBy(p => p.ChildArticleName).ToList();

                return Result<DetailsDto>.Success(article);
            }
        }
    }
}