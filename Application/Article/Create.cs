using System.Diagnostics;
using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;


namespace Application.Article
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string FullName { get; set; }
            public string NameWithoutFamilly { get; set; }
            public int ArticleTypeId { get; set; }
            public int? FamillyId { get; set; }
            public int? StuffId { get; set; }
            public int? Length { get; set; }
            public int? Width { get; set; }
            public int? High { get; set; }
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
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Articles.Any(p => p.FullName.ToUpper() == request.FullName.ToUpper()
                                             && p.ArticleTypeId == request.ArticleTypeId
                                             && p.FamillyId == request.FamillyId
                                             && p.StuffId == request.StuffId))
                {
                    return Result<Unit>.Failure("Article with choosen parameters exist in DataBase");
                }
                var articleType = await _context.ArticleTypes.FirstOrDefaultAsync(p => p.Id == request.ArticleTypeId);
                if (articleType == null) return null;
                Domain.Familly familly = null;
                Domain.Stuff stuff = null;

                if (request.FamillyId != 0)
                {
                    familly = await _context.Famillies.FirstOrDefaultAsync(p => p.Id == request.FamillyId);
                    if (familly == null) return null;
                }
                if (request.StuffId != 0)
                {
                    stuff = await _context.Stuffs.FirstOrDefaultAsync(p => p.Id == request.StuffId);
                    if (stuff == null) return null;
                }

                var properties = Relations.ArticleProperties.FirstOrDefault(p=>p.ArticleTypeId==articleType.Id);

                if(properties.HasFamilly && familly==null)
                     return Result<Unit>.Failure("Article needs familly");
                if(properties.HasStuff && stuff==null)
                     return Result<Unit>.Failure("Article needs stuff");

                var article = new Domain.Article
                {
                    FullName = request.FullName,
                    NameWithoutFamilly = request.NameWithoutFamilly,
                    ArticleType = articleType,
                    ArticleTypeId = articleType.Id,
                    Familly = properties.HasFamilly ? familly : null,
                    FamillyId = properties.HasFamilly ? familly.Id : null,
                    Stuff = properties.HasStuff ? stuff : null,
                    StuffId = properties.HasStuff ? stuff.Id : null,
                    EditDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    Length = request.Length == null ? 0 : (int)request.Length,
                    Width = request.Width == null ? 0 : (int)request.Width,
                    High = request.High == null ? 0 : (int)request.High,

                };

                article.CalculateCapacity();
                _context.Articles.Add(article);

                var articleComponents = new List<Domain.ArticleArticle>();
                var possibleChildTypes = Relations.ArticleTypeRelations.Where(p=>p.Parent==articleType.Id).Select(p=>p.Child).ToList();
                if (request.ChildArticles!=null && request.ChildArticles.Count > 0)
                {
                    try
                    {
                        var componentsIds = request.ChildArticles.Select(p => p.ChildId).ToList();
                        var articlesToAssing = await _context.Articles.Where(p => componentsIds.Contains(p.Id)).ToListAsync();
                        foreach (var component in request.ChildArticles)
                        {
                            var componentDB = articlesToAssing.FirstOrDefault(p => p.Id == component.ChildId);
                            if (componentDB == null) return null;
                            if(!possibleChildTypes.Any(p=>p==componentDB.ArticleTypeId))
                                return Result<Unit>.Failure($"Article type of article {componentDB.FullName} is not right for the article type you want to create");
                            articleComponents.Add(new Domain.ArticleArticle { ParentArticle = article, ParentId = article.Id, ChildArticle = componentDB, ChildId = componentDB.Id, Quanity = component.Quanity });
                        }
                        if (articleComponents.Count > 0)
                            _context.ArticleArticle.AddRange(articleComponents);
                    }catch(Exception e){
                        return Result<Unit>.Failure($"Failed to create Article: {e.Message}");
                    }
                }
                if(articleComponents.Count>0)
                    article.HasChild=true;


                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create Article");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}