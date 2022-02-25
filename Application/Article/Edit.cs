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
            public int Id {get;set;}
            public string FullName { get; set; }
            public string NameWithoutFamilly { get; set; }
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
                var article= await _context.Articles.FirstOrDefaultAsync(p=>p.Id==request.Id);

                if(article==null) return null;

                if(_context.Articles.Any(p=>p.FullName.ToUpper()==request.FullName.ToUpper() 
                && p.ArticleTypeId==article.ArticleTypeId && p.FamillyId==article.FamillyId 
                && p.StuffId==article.StuffId))
                {
                    return Result<Unit>.Failure("Article with those parameters exist in database");
                }
                article.FullName=request.FullName;
                article.NameWithoutFamilly=request.NameWithoutFamilly;
                article.Length=request.Length==null ? article.Length : (int)request.Length;
                article.Width=request.Width==null ? article.Width : (int)request.Width;
                article.High=request.High==null ? article.High : (int)request.High;
                article.CreatedInCompany=request.CreatedInCompany;
                article.EditDate=DateTime.Now;
                article.CalculateCapacity();

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit Article");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}