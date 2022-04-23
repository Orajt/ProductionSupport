using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Article
{
    public class CreateFabric
    {
         public class Command : IRequest<Result<Unit>>
        {
            public string FullName { get; set; }
            public int StuffId { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FullName.Length).GreaterThan(0);
                RuleFor(x => x.StuffId).NotNull().GreaterThan(0);
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
                                             && p.ArticleTypeId == 6))
                {
                    return Result<Unit>.Failure("Fabrics with that name exists in database");
                }
                var stuff = await _context.Stuffs.FirstOrDefaultAsync(p=>p.Id==request.StuffId);
                if(stuff==null) return null;

                var date = DateTime.Now.Date;
                var article = new Domain.Article
                {
                    FullName = request.FullName,
                    NameWithoutFamilly = request.FullName,
                    ArticleTypeId = 6,
                    EditDate = date,
                    CreateDate = date,
                    CreatedInCompany=false,
                    HasChild=false,
                    Stuff=stuff,
                    StuffId=stuff.Id
                };

                _context.Articles.Add(article);


                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create Fabric");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}