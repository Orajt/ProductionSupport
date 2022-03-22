using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string NameWithoutFamilly { get; set; }
            public int FamillyId { get; set; }
            public int StuffId { get; set; }
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
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var article = await _context.Articles.FirstOrDefaultAsync(p => p.Id == request.Id);

                if (article == null) return null;

                 if (_context.Articles.Any(p => p.FullName.ToUpper() == request.FullName.ToUpper()
                                             && p.ArticleTypeId == article.ArticleTypeId
                                             && p.FamillyId == request.FamillyId
                                             && p.StuffId == request.StuffId))
                {
                    return Result<Unit>.Failure("Article with choosen parameters exist in DataBase");
                }

                Domain.Familly familly = article.Familly;
                Domain.Stuff stuff = article.Stuff;

                if (request.FamillyId != 0 && request.FamillyId!=article.FamillyId)
                {
                    familly = await _context.Famillies.FirstOrDefaultAsync(p => p.Id == request.FamillyId);
                    if (familly == null) return null;
                    article.Familly=familly;
                    article.FamillyId=familly.Id;
                }
                if (request.StuffId != 0 && request.StuffId!=article.StuffId)
                {
                    stuff = await _context.Stuffs.FirstOrDefaultAsync(p => p.Id == request.StuffId);
                    if (stuff == null) return null;
                    article.Stuff=stuff;
                    article.StuffId=stuff.Id;
                }

                article.Width=request.Width;
                article.Length=request.Length;
                article.High=request.High;
                article.CreatedInCompany=request.CreatedInCompany;
                article.CalculateCapacity();
                article.FullName=request.FullName;
                article.NameWithoutFamilly=request.NameWithoutFamilly;
                
                var possibleChildTypes = Relations.ArticleTypeRelations.Where(p=>p.Parent==article.ArticleTypeId).Select(p=>p.Child).ToList();
                var requestCompoentIds=request.ChildArticles.Select(p=>p.ChildId).ToList();

                var articleDependenciesToDelete = article.ChildRelations.Where(p=>!requestCompoentIds.Contains(p.ChildId)).ToList();
                _context.ArticleArticle.RemoveRange(articleDependenciesToDelete);

                var articleComponents = article.ChildRelations.Where(p=>requestCompoentIds.Contains(p.ChildId)).ToList();

                var newArticleIds=request.ChildArticles
                    .Where(p=>articleComponents.Select(p=>p.ChildId).Contains(p.ChildId))
                    .Select(p=>p.ChildId).ToList();
                
                var newArticlesToAssign = await _context.Articles.Where(p=>newArticleIds.Contains(p.Id)).ToListAsync();
                var articleDependenciesToAdd = new List<Domain.ArticleArticle>();

                if(request.ChildArticles!=null && request.ChildArticles.Count>0)
                {
                    foreach(var component in request.ChildArticles)
                    {
                        var oldComponent = articleComponents.FirstOrDefault(p=>p.ChildId==component.ChildId);
                        if(oldComponent!=null){
                            oldComponent.Quanity=component.Quanity;
                            continue;
                        }
                        var articleToAssign = newArticlesToAssign.FirstOrDefault(p=>p.Id==component.ChildId);
                        if(articleToAssign==null) return null;
                        articleDependenciesToAdd.Add(new Domain.ArticleArticle{
                            ParentArticle=article,
                            ParentId=article.Id,
                            ChildArticle=articleToAssign,
                            ChildId=articleToAssign.Id,
                            Quanity=component.Quanity
                        });
                    }
                }
                _context.ArticleArticle.AddRange(articleDependenciesToAdd);

                article.HasChild = (articleDependenciesToAdd.Count>0 || articleComponents.Count>0);
                var result = await _context.SaveChangesAsync() > 0;
               
                if (!result) return Result<Unit>.Failure("Failed to edit Article");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}