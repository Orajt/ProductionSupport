using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int DeliveryPlaceId { get; set; }
            public DateTime ShipmentDate { get; set; }
            public DateTime ProductionDate { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Name).NotNull();
                RuleFor(p => p.DeliveryPlaceId).NotNull();

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
                if (await _context.Orders.AsNoTracking()
                    .AnyAsync(p => p.Name.ToUpper() == request.Name.ToUpper()))
                    return Result<Unit>.Failure($"Order named {request.Name} exist in database");

                var order = await _context.Orders.FirstOrDefaultAsync(p => p.Id == request.Id);
                if (order.DeliveryPlaceId != request.DeliveryPlaceId)
                {
                    var deliveryPlace = await _context.DeliveryPlaces.FirstOrDefaultAsync(p => p.Id == request.DeliveryPlaceId);
                    if (deliveryPlace==null) return null;
                    order.DeliveryPlace = deliveryPlace;
                    order.DeliveryPlaceId = deliveryPlace.Id;
                }

                order.Name = request.Name;
                order.EditDate = DateTime.Now;
                order.ShipmentDate = request.ShipmentDate;
                order.ProductionDate = request.ProductionDate;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit Order");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}