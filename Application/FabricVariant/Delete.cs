using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FabricVariant
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
               RuleFor(p=>p.Id).NotNull();
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
                var fabricVariant = await _context.FabricVariants
                    .Include(p=>p.FabricVariantGroups)
                    .FirstOrDefaultAsync(p=>p.Id==request.Id);
                
                if(fabricVariant.FabricVariantGroups.Any())
                    return Result<Unit>.Failure("Fabric variant is used in one or more groups");
                
                _context.Remove(fabricVariant);

                 var result = await _context.SaveChangesAsync() > 0;
                
                if (!result) return Result<Unit>.Failure("Failed to delete fabric variant");
                
                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}