using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Article
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string NameWithoutFamilly { get; set; }
            public int? FamillyId { get; set; }
            public int? StuffId { get; set; }
            public int? FabricVariantGroupId { get; set; }
            public int Length { get; set; }
            public int Width { get; set; }
            public int High { get; set; }
            public bool CreatedInCompany { get; set; }
            public List<DetailsDtoChildArticles> ChildArticles { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FullName.Length).GreaterThan(0);
                RuleFor(x => x.NameWithoutFamilly.Length).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IArticleHelpers _articleHelpers;
            public Handler(IUnitOfWork unitOfWork, IArticleHelpers articleHelpers)
            {
                _articleHelpers = articleHelpers;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var article = await _unitOfWork.Articles.GetArticleWithChildRelationsById(request.Id);
                if (article == null) return null;
                if (article.FullName != request.FullName &&
                    await _unitOfWork.Articles.IsArticleNameUsed(request.FullName, article.ArticleTypeId, request.StuffId))
                {
                    return Result<Unit>.Failure("Article with choosen parameters exist in DataBase");
                }
                var newArticleProperties = await _unitOfWork.Articles.FindAdditionalProperties(
                    request.FamillyId != article.FamillyId ? request.FamillyId : null,
                    request.StuffId != article.StuffId ? request.StuffId : null,
                    request.FabricVariantGroupId != article.FabricVariantGroupId ? request.FabricVariantGroupId : null);

                if (article.FabricVariantGroupId != request.FabricVariantGroupId)
                {
                    var fabricRealizationsToDelete = await _unitOfWork.ArticlesFabricRealizations.Where(p => p.ArticleId == article.Id);
                    _unitOfWork.ArticlesFabricRealizations.RemoveRange(fabricRealizationsToDelete);
                }
                article = _articleHelpers.ReplaceArticleProperitiesToNewer(article, newArticleProperties, request);

                if (request.ChildArticles == null || request.ChildArticles.Count == 0)
                {
                    _unitOfWork.ArticlesArticles.RemoveRange(article.ChildRelations);
                    article.HasChild = false;
                }

                if (request.ChildArticles != null && request.ChildArticles.Count > 0)
                {
                    var requestCompoentIds = request.ChildArticles.Select(p => p.ChildId).ToList();

                    //Delete article components that arent on the request child articles list//
                    var articleComponentsToDelete = article.ChildRelations.Where(p => !requestCompoentIds.Contains(p.ChildId)).ToList();
                    _unitOfWork.ArticlesArticles.RemoveRange(articleComponentsToDelete);

                    //Select components that were before and still exists//
                    var articleOldComponents = article.ChildRelations.Where(p => requestCompoentIds.Contains(p.ChildId)).ToList();

                    if (request.ChildArticles != null && request.ChildArticles.Count > 0)
                    {
                        foreach (var component in articleOldComponents)
                        {
                            component.Quanity = request.ChildArticles.FirstOrDefault(p => p.ChildId == component.ChildId).Quanity;
                        }
                    }

                    //New components to add//
                    var articleNewComponents = request.ChildArticles
                        .Where(p => !articleOldComponents.Select(z => z.ChildId).Contains(p.ChildId)).ToList();

                    if (articleNewComponents.Count > 0)
                    {
                        var articleNewComponentsToAdd = await _unitOfWork.ArticlesArticles.GetComponentsToParentAricle(components: articleNewComponents, parent: article);
                        if (articleNewComponentsToAdd.Count == 0)
                            return Result<Unit>.Failure($"One or more child article types doesnt match to parent article");
                        if (articleNewComponentsToAdd.Count > 0)
                        {
                            _unitOfWork.ArticlesArticles.AddRange(articleNewComponentsToAdd);
                            article.HasChild = true;
                        }
                    }
                    article.HasChild = true;
                }
                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to edit Article");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}