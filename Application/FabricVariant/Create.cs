using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariant
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Name { get; set; }
            public string ShortName{get;set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Name.Count()).GreaterThan(0);
                RuleFor(p => p.ShortName.Count()).GreaterThan(0);
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
                if (await _context.FabricVariants.AnyAsync(p => p.FullName == request.Name.ToUpper() || p.ShortName== request.ShortName.ToUpper()))
                    return Result<Unit>.Failure($"Fabric variant named {request.Name}, or shortname {request.ShortName} exists in database");


                var newFabricVariant = new Domain.FabricVariant
                {
                    FullName = request.Name.ToUpper(),
                    ShortName = request.ShortName.ToUpper(),
                };

                _context.FabricVariants.Add(newFabricVariant);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create stuff");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}