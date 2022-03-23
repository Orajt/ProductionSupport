using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Company
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
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
                var company=await _context.Companies.FirstOrDefaultAsync(p=>p.Id==request.Id);
                if(company==null) return null;

                if(request.CompanyIdentifier.ToUpper()!=company.CompanyIdentifier.ToUpper())
                {
                    if(await _context.Companies.AnyAsync(p=>p.CompanyIdentifier.ToUpper()==request.CompanyIdentifier.ToUpper()))
                        return Result<Unit>.Failure($"Company identifier exist in database");
                    company.CompanyIdentifier=request.CompanyIdentifier;
                }
                if(request.Name.ToUpper()!=company.Name.ToUpper())
                {
                    if(await _context.Companies.AnyAsync(p=>p.Name.ToUpper()==request.Name.ToUpper()))
                        return Result<Unit>.Failure($"Company identifier exist in database");
                    company.Name=request.Name;
                }
                
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create stuff");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}