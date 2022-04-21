using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class OrderDone
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int OrderId { get; set; }
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
                var order = await _context.Orders.FirstOrDefaultAsync(p=>p.Id==request.OrderId);

                if(order==null) return null;

                order.Done=!order.Done;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to chaange order");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}