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
                var result = new List<ReactSelectInt>();
                var deliveryPlaces=new List<Domain.DeliveryPlace>();
                switch (request.Predicate)
                {
                    case "SELLER":
                        deliveryPlaces = await _context.DeliveryPlaces.Include(p=>p.Company)
                            .Where(p => p.Company.Supplier==true)
                            .OrderBy(p=>p.Name)
                            .ToListAsync();
                        break;
                    case "DEALER":
                        deliveryPlaces = await _context.DeliveryPlaces.Include(p=>p.Company)
                            .Where(p => p.Company.Merchant==true)
                            .OrderBy(p=>p.Name)
                            .ToListAsync();
                        break;
                    case "ALL":
                        deliveryPlaces = await _context.DeliveryPlaces.Include(p=>p.Company)
                            .OrderBy(p=>p.Name)
                            .ToListAsync();
                        break;
                    default:
                        return Result<List<ReactSelectInt>>.Failure("Choosen article type doesn't have any article assigned or predicate is wrong");
                }
                foreach(var deliveryPlace in deliveryPlaces){
                    result.Add(new ReactSelectInt{Label=$"{deliveryPlace.Name}({deliveryPlace.Company.Name})", Value=deliveryPlace.Id});
                }

                return Result<List<ReactSelectInt>>.Success(result);
            }
        }
    }
}