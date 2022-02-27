using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Name { get; set; }
            public int DeliveryPlaceId { get; set; }
            public DateTime ShipmentDate { get; set; }
            public DateTime ProductionDate{get;set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(p=>p.Name).NotNull();
               RuleFor(p=>p.DeliveryPlaceId).NotNull();

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
                if(await _context.Orders.AsNoTracking()
                    .AnyAsync(p=>p.Name.ToUpper()==request.Name.ToUpper()))
                    return Result<Unit>.Failure($"Order named {request.Name} exist in database");
                var deliveryPlace=await _context.DeliveryPlaces.FirstOrDefaultAsync(p=>p.Id==request.DeliveryPlaceId);
                await _context.Orders.AddAsync(new Domain.Order{
                    Name=request.Name,
                    EditDate=DateTime.Now,
                    ShipmentDate=request.ShipmentDate,
                    ProductionDate=request.ProductionDate,
                    DeliveryPlace=deliveryPlace,
                    DeliveryPlaceId=deliveryPlace.Id,                  
                });
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create Order");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}