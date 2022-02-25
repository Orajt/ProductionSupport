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
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                if (_context.Articles.Any(p => p.FullName.ToUpper() == request.FullName.ToUpper()
                                             && p.ArticleTypeId == request.ArticleTypeId
                                             && p.FamillyId == request.FamillyId
                                             && p.StuffId == request.StuffId))
                {
                    return Result<Unit>.Failure("Article with choosen parameters exist in DataBase");
                }

                Domain.Familly familly = null;
                Domain.Stuff stuff = null;
                var articleType = await _context.ArticleTypes.FirstOrDefaultAsync(p => p.Id == request.ArticleTypeId);
                if (articleType == null) return null;

                if (request.FamillyId != null)
                {
                    familly = await _context.Famillies.FirstOrDefaultAsync(p => p.Id == request.FamillyId);
                    if (familly == null) return null;
                }
                if (request.StuffId != null)
                {
                    stuff = await _context.Stuffs.FirstOrDefaultAsync(p => p.Id == request.StuffId);
                    if (stuff == null) return null;
                }
                var article = new Domain.Article
                {
                    FullName = request.FullName,
                    NameWithoutFamilly = request.NameWithoutFamilly,
                    ArticleType = articleType,
                    ArticleTypeId = articleType.Id,
                    Familly = familly,
                    FamillyId = familly == null ? null : familly.Id,
                    Stuff = stuff,
                    StuffId = stuff == null ? null : stuff.Id,
                    EditDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    Length = request.Length == null ? 0 : (int)request.Length,
                    Width = request.Width == null ? 0 : (int)request.Width,
                    High = request.High == null ? 0 : (int)request.High,

                };
                article.CalculateCapacity();
                await _context.Articles.AddAsync(article);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create Article");
                watch.Stop();
                Debug.WriteLine($"Execution time:{watch.ElapsedMilliseconds} milliseconds");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}