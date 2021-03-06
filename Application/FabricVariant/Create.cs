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
            public string FullName { get; set; }
            public string ShortName{get;set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.FullName.Count()).GreaterThan(0);
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
                if (await _context.FabricVariants.AnyAsync(p => p.FullName == request.FullName.ToUpper() || p.ShortName== request.ShortName.ToUpper()))
                    return Result<Unit>.Failure($"Fabric variant named {request.FullName}, or shortname {request.ShortName} exists in database");

                if(request.ShortName.Length>3)  
                    return Result<Unit>.Failure($"Shortname max length is 3 characters");

                var newFabricVariant = new Domain.FabricVariant
                {
                    FullName = request.FullName.ToUpper(),
                    ShortName = request.ShortName.ToUpper(),
                };

                _context.FabricVariants.Add(newFabricVariant);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create fabric variant");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}