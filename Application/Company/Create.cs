using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Company
{
    public class Create
    {
         public class Command : IRequest<Result<Unit>>
        {
            public string Name { get; set; }
            public string CompanyIdentifier { get; set; }
            public bool Supplier { get; set; }
            public bool Merchant { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(p=>p.Name.Count()).GreaterThan(0);
               RuleFor(p=>p.CompanyIdentifier.Count()).GreaterThan(0);
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
                if(await _context.Companies.AnyAsync(p=>p.Name.ToUpper()==request.Name.ToUpper() || p.CompanyIdentifier.ToUpper()==request.CompanyIdentifier.ToUpper()))
                    return Result<Unit>.Failure($"Company named {request.Name}, or company identifier exist in database");


                var newComapny = new Domain.Company{
                    Name=request.Name,  
                    CompanyIdentifier=request.CompanyIdentifier,
                    Supplier=request.Supplier,
                    Merchant=request.Merchant
                };

                _context.Companies.Add(newComapny);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create stuff");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}