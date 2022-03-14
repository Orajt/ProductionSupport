using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DeliveryPlace
{
    public class ListReactSelect
    {
        public class Query : IRequest<Result<List<ReactSelectInt>>>
        {
            public string Predicate{get;set;}
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
                var deliveryPlaces = new List<ReactSelectInt>();
                switch (request.Predicate)
                {
                    case "SELLER":
                        deliveryPlaces = await _context.DeliveryPlaces.Include(p=>p.Company)
                            .AsNoTracking()
                            .Where(p => p.Company.Seller==true)
                            .Select(p => new ReactSelectInt { Label = p.NameWithCompany, Value = p.Id })
                            .ToListAsync();
                        break;
                    case "DEALER":
                        deliveryPlaces = await _context.DeliveryPlaces.Include(p=>p.Company)
                            .AsNoTracking()
                            .Where(p => p.Company.Seller==false)
                            .Select(p => new ReactSelectInt { Label = p.NameWithCompany, Value = p.Id })
                            .ToListAsync();
                        break;
                    case "ALL":
                        deliveryPlaces = await _context.DeliveryPlaces.Include(p=>p.Company)
                            .AsNoTracking()
                            .Select(p => new ReactSelectInt { Label = p.NameWithCompany, Value = p.Id })
                            .ToListAsync();
                        break;
                    default:
                        return Result<List<ReactSelectInt>>.Failure("Choosen article type doesn't have any article assigned or predicate is wrong");
                }

                return Result<List<ReactSelectInt>>.Success(deliveryPlaces);
            }
        }
    }
}