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
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(p=>p.Name).NotNull();
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
                if(await _context.Stuffs.AnyAsync(p=>p.Name.ToUpper()==request.Name.ToUpper()))
                    return Result<Unit>.Failure($"That stuff {request.Name} exist in database");

                var newStuff = new Domain.Stuff{
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