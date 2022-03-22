using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id{get;set;}
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(p=>p.Id).NotNull().GreaterThan(0);
               RuleFor(p=>p.Name.Count()).GreaterThan(0);
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
                var stuff = await _context.Stuffs.FirstOrDefaultAsync(p=>p.Id==request.Id);

                if(stuff==null) return null;

                if(await _context.Stuffs.AnyAsync(p=>p.Name.ToUpper()==request.Name.ToUpper() 
                && p.ArticleTypeId==stuff.ArticleTypeId))
                    return Result<Unit>.Failure($"That stuff {request.Name} exist in database");

                stuff.Name=request.Name;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit stuff");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}