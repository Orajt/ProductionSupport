using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Article
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var article = await _unitOfWork.Articles.GetArticleWithChildAndParentRelations(request.Id);

                if (article == null) return null;

                if (await _unitOfWork.OrderPositions.AnyPositionsWithArticleId(article.Id))
                    Result<Unit>.Failure("You cant delete article that was used in ordered");

                _unitOfWork.ArticlesArticles.RemoveRange(article.ChildRelations.Concat(article.ParentRelations));
                _unitOfWork.Articles.Remove(article);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to delete Article");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}