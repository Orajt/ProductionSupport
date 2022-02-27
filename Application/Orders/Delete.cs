using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
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
                var order = await _context.Orders
                    .Include(p=>p.OrderPositions)
                    .ThenInclude(p=>p.Realizations)
                    .FirstOrDefaultAsync(p=>p.Id==request.Id);

                if(order.Done==true) Result<Unit>.Failure("Order is done");

                _context.OrderPositionRealizations.RemoveRange(order.OrderPositions.SelectMany(p=>p.Realizations).ToList());
                _context.OrderPositions.RemoveRange(order.OrderPositions);
                _context.Orders.Remove(order);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete Order");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}