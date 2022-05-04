using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class ListReactSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
            public int ArticleTypeId { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ReactSelectInt>>>
        {
            private readonly IRelations _relations;
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IRelations relations, IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _relations = relations;
            }

            public async Task<Result<List<ReactSelectInt>>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.ArticleTypeId != 0)
                {
                    var articleType = await _unitOfWork.ArticleTypes.Find(request.ArticleTypeId);
                    if (articleType == null) return null;
                }

                var articlesRS = new List<ReactSelectInt>();
                switch (request.Predicate)
                {
                    case "TOASSIGN":
                        var possibleTypes = _relations.ArticleTypesIdsPossibleToAssign(request.ArticleTypeId);
                        articlesRS = await _unitOfWork.Articles.GetArticlesPossibleToAssignToArticleType(request.ArticleTypeId, possibleTypes);
                        break;
                    case "FULLLIST":
                        articlesRS = await _unitOfWork.Articles.GetAllArticlesBelongsToArticleType(request.ArticleTypeId);
                        break;
                    default:
                        return Result<List<ReactSelectInt>>.Failure("Choosen article type doesn't have any article assigned or predicate is wrong");
                }

                return Result<List<ReactSelectInt>>.Success(articlesRS);
            }
        }
    }
}