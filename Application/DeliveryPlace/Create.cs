using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DeliveryPlace
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Name { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public int NumberOfBuilding { get; set; }
            public int Apartment { get; set; } = 0;
            public int CompanyID { get; set; }
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
                RuleFor(p => p.CompanyID).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.DeliveryPlaces.AnyAsync(p => p.Name.ToUpper() == request.Name.ToUpper()))
                    return Result<Unit>.Failure($"Delivery place {request.Name} exist in database");

                var company = await _context.Companies.FirstOrDefaultAsync(p => p.Id == request.CompanyID);

                if (company == null) return null;

                var newDeliveryPlace = _mapper.Map<Domain.DeliveryPlace>(request);

                newDeliveryPlace.Company = company;

                _context.DeliveryPlaces.Add(newDeliveryPlace);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create delivery place");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}