using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;


namespace Application.Article
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string FullName { get; set; }
            public string NameWithoutFamilly { get; set; }
            public int ArticleTypeId { get; set; }
            public int? FamillyId { get; set; } = 0;
            public int? StuffId { get; set; } = 0;
            public int? FabricVariantGroupId { get; set; } = 0;
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
                RuleFor(x => x.ArticleTypeId).NotNull().GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRelations _relations;
            private readonly IArticleHelpers _articleHelpers;
            public Handler(IUnitOfWork unitOfWork, IRelations relations, IArticleHelpers articleHelpers)
            {
                _articleHelpers = articleHelpers;
                _relations = relations;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _unitOfWork.Articles.IsArticleNameUsed(request.FullName, request.ArticleTypeId, request.StuffId))
                {
                    return Result<Unit>.Failure("Article with choosen parameters exist in DataBase");
                }
                var articleType = await _unitOfWork.ArticleTypes.Find(request.ArticleTypeId);
                if (articleType == null)
                    return null;

                var articleProperties = await _unitOfWork.Articles.FindAdditionalProperties(request.FamillyId, request.StuffId, request.FabricVariantGroupId);
                var articleTypeProperties = _relations.ArticleProperties(articleType.Id);
                var propertiesError = _articleHelpers.ArticleProperitiesError(articleTypeProperties, articleProperties);

                if (!String.IsNullOrEmpty(propertiesError))
                {
                    return Result<Unit>.Failure(propertiesError);
                }
                var article = new Domain.Article
                {
                    FullName = request.FullName,
                    NameWithoutFamilly = request.NameWithoutFamilly,
                    ArticleType = articleType,
                    ArticleTypeId = articleType.Id,
                    Familly = articleTypeProperties.HasFamilly ? articleProperties.Familly : null,
                    FamillyId = articleTypeProperties.HasFamilly ? articleProperties.Familly.Id : null,
                    Stuff = articleTypeProperties.HasStuff ? articleProperties.Stuff : null,
                    StuffId = articleTypeProperties.HasStuff ? articleProperties.Stuff.Id : null,
                    FabricVariant = articleTypeProperties.HasFabicVariantGroup ? articleProperties.FabricVariantGroup : null,
                    FabricVariantGroupId = articleTypeProperties.HasFabicVariantGroup ? articleProperties.FabricVariantGroup.Id : null,
                    EditDate = DateHelpers.SetDateTimeToCurrent(DateTime.Now).Date,
                    CreateDate = DateHelpers.SetDateTimeToCurrent(DateTime.Now).Date,
                    Length = request.Length,
                    Width = request.Width,
                    High = request.High,
                    CreatedInCompany = request.CreatedInCompany
                };

                article.CalculateCapacity();
                _unitOfWork.Articles.Add(article);

                if (request.ChildArticles != null && request.ChildArticles.Count > 0)
                {
                    try
                    {
                        var articleComponents = await _unitOfWork.ArticlesArticles.GetComponentsToParentAricle(components: request.ChildArticles, parent: article);
                        if (articleComponents.Count == 0)
                            return Result<Unit>.Failure($"One or more child article types doesnt match to parent article");

                        _unitOfWork.ArticlesArticles.AddRange(articleComponents);
                        article.HasChild = true;
                    }
                    catch (Exception e)
                    {
                        return Result<Unit>.Failure($"Failed to create assign child articles: {e.Message}");
                    }
                }
                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to create Article");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}