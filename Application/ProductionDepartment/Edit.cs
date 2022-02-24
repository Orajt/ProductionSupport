using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.ProductionDepartment
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Domain.ProductionDepartment ProductionDepartment { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductionDepartment).NotNull();
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
                var productionDepartment = await _context.ProductionDepartments.FindAsync(request.ProductionDepartment.Id);

                if (productionDepartment == null) return null;

                productionDepartment.Name=request.ProductionDepartment.Name;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update Production Department");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}