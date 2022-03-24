using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Company
{
    public class Details
    {
        public class Query : IRequest<Result<DetailsDto>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<DetailsDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<DetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                Domain.Company company=null;
                var companyId=0;
                if(int.TryParse(request.Id, out companyId))
                    company = await _context.Companies.Include(p=>p.DeliveryPlaces).FirstOrDefaultAsync(p=>p.Id==companyId);
                if(companyId==0)
                    company = await _context.Companies.Include(p=>p.DeliveryPlaces).FirstOrDefaultAsync(p=>p.Name==request.Id);
                if(company==null) return null;
                var result = new DetailsDto(){
                    Id=company.Id,
                    Name=company.Name,
                    CompanyIdentifier=company.CompanyIdentifier,
                    Supplier=company.Supplier,
                    Merchant=company.Merchant
                };
                foreach(var deliveryPlace in company.DeliveryPlaces)
                {
                    result.DeliveryPlaces.Add(new DeliveryPlace.ListDto(){
                        Id=deliveryPlace.Id,
                        Name=deliveryPlace.Name,
                        FullAdress=deliveryPlace.FullAdress,
                        CompanyName=company.Name
                    });
                }

                return Result<DetailsDto>.Success(result);
            }
        }
    }
}