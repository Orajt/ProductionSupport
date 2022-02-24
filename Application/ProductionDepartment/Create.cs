using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.ProductionDepartment
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Domain.ProductionDepartment ProductionDepartment { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductionDepartment.Name.Length).GreaterThan(0);
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
                
                if(_context.ProductionDepartments.Any(p=>p.Name.ToUpper()==request.ProductionDepartment.Name.ToUpper()))
                {
                    return Result<Unit>.Failure($"Production department named {request.ProductionDepartment.Name} exist in database");
                }

                _context.ProductionDepartments.Add(request.ProductionDepartment);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create Production Department");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}