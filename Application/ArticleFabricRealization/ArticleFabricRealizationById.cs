using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ArticleFabricRealization
{
    public class ListFabricRealizationById
    {
        public class Query : IRequest<Result<ArticleFRDetailsDto>>
        {
            public int ArticleId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ArticleFRDetailsDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<ArticleFRDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var article = await _unitOfWork.Articles.GetArticleWithFabricVarGroupAndRealizations(request.ArticleId);

                if (article == null) return null;

                var fabricRealizationsGrouped = article.Realizations
                    .OrderBy(p => p.StuffId)
                    .GroupBy(p => p.StuffId)
                    .ToList();


                var result = new ArticleFRDetailsDto();
                result.ArticleName = article.FullName;
                result.VariantGroup = article.FabricVariant.Name;

                foreach (var stuffGroup in fabricRealizationsGrouped)
                {
                    var newGroup = new ArticleFRByStuff()
                    {
                        StuffName = stuffGroup.First().Stuff.Name,
                        StuffId = stuffGroup.Key
                    };

                    foreach (var group in stuffGroup)
                    {
                        newGroup.GroupsQuanities.Add(new QuanityPerGroup
                        {
                            GroupId = group.Id,
                            CalculatedCode = group.CalculatedCode,
                            StuffId = stuffGroup.Key,
                            QuanityChaanged = false,
                            Quanity = group.FabricLength
                        });
                    }
                    newGroup.GroupsQuanities.OrderBy(p => p.CalculatedCode).ToList();
                    result.GroupByStuffList.Add(newGroup);
                }
                return Result<ArticleFRDetailsDto>.Success(result);
            }
        }
    }
}