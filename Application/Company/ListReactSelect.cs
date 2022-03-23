using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Company
{
    public class ListReactSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ReactSelectInt>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ReactSelectInt>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companies = new List<Domain.Company>();

                if (request.Predicate == "MERCHANT")
                    companies = await _context.Companies.Where(p => p.Merchant == true).ToListAsync();
                else if (request.Predicate == "SUPPLIER")
                    companies = await _context.Companies.Where(p => p.Supplier == true).ToListAsync();
                else
                    companies = await _context.Companies.ToListAsync();

                var companiesReactSelect = new List<ReactSelectInt>();

                foreach (var company in companies)
                {
                    companiesReactSelect.Add(new ReactSelectInt
                    {
                        Label = $"{company.Name}({company.CompanyIdentifier})",
                        Value = company.Id
                    });
                }
                return Result<List<ReactSelectInt>>.Success(companiesReactSelect);
            }

        }
    }
}