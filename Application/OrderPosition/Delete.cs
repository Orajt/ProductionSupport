using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.OrderPosition
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int OrderId { get; set; }
            public List<long> PositionsToRemove { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p=>p.PositionsToRemove.Count).NotNull().GreaterThan(0);
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
                try
                {
                    await _context.OrderPositions.Where(p => p.OrderId == request.OrderId && request.PositionsToRemove.Contains(p.Id)).DeleteFromQueryAsync();
                }
                catch (Exception)
                {
                    return Result<Unit>.Failure("Failed to delete order positions");
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
