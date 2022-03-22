using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Stuff
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(p=>p.Id).NotNull().GreaterThan(0);
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
                if(await _context.Articles.AnyAsync(p=>p.StuffId==request.Id))
                {
                    return Result<Unit>.Failure("Stuff is being used in some articles");
                }
                var stuff = await _context.Stuffs.FirstOrDefaultAsync(p=>p.Id==request.Id);
                
                if(stuff==null)
                    return null;

                _context.Stuffs.Remove(stuff);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create stuff");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}