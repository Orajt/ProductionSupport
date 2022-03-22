using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Name { get; set; }
            public int ArticleTypeId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(p=>p.Name).NotNull();
               RuleFor(p=>p.ArticleTypeId).GreaterThan(0);
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
                if(await _context.Stuffs.AnyAsync(p=>p.Name.ToUpper()==request.Name.ToUpper() && p.ArticleTypeId==request.ArticleTypeId))
                    return Result<Unit>.Failure($"That stuff {request.Name} exist in database");

                var articleType = await _context.ArticleTypes.FirstOrDefaultAsync(p=>p.Id==request.ArticleTypeId);
                if(articleType==null) return null;

                var newStuff = new Domain.Stuff{
                    ArticleType=articleType,
                    ArticleTypeId=articleType.Id,
                    Name=request.Name,  
                };

                _context.Stuffs.Add(newStuff);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create stuff");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}