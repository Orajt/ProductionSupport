using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DeliveryPlace
{
    public class Edit
    {
         public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }    
            public string Name { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public int NumberOfBuilding { get; set; }
            public int Apartment { get; set; } = 0;
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Name.Count()).GreaterThan(0);
                RuleFor(p => p.Country.Count()).GreaterThan(0);
                RuleFor(p => p.City.Count()).GreaterThan(0);
                RuleFor(p => p.Street.Count()).GreaterThan(0);
                RuleFor(p => p.PostalCode.Count()).GreaterThan(0);
                RuleFor(p => p.NumberOfBuilding).GreaterThan(0);
                RuleFor(p => p.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var deliveryPlace=await _context.DeliveryPlaces.FirstOrDefaultAsync(p=>p.Id==request.Id);

                if(deliveryPlace==null) return null;

                if (request.Name.ToUpper()!=deliveryPlace.Name.ToUpper() && await _context.DeliveryPlaces.AnyAsync(p => p.Name.ToUpper() == request.Name.ToUpper()))
                    return Result<Unit>.Failure($"Delivery place {request.Name} exist in database");

                deliveryPlace.Name=request.Name;
                deliveryPlace.Country=request.Country;
                deliveryPlace.City=request.City;
                deliveryPlace.Street=request.Street;
                deliveryPlace.PostalCode=request.PostalCode;
                deliveryPlace.NumberOfBuilding=request.NumberOfBuilding;
                deliveryPlace.Apartment=request.Apartment;
                
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create delivery place");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}