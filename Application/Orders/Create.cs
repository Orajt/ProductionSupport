using Application.Core;
using Domain;
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
            public List<OrderPosition.PositionDto> OrderPositions{get;set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(p=>p.Name).NotNull();
               RuleFor(p=>p.DeliveryPlaceId).NotNull();
               RuleFor(p=>p.ShipmentDate).NotNull();
               RuleFor(p=>p.ProductionDate).NotNull();

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
                if(deliveryPlace==null) return null;

                var newOrder=new Domain.Order{
                    Name=request.Name,
                    EditDate=DateTime.Now,
                    ShipmentDate=DateHelpers.SetDateOnlyDaysMonthYear(request.ShipmentDate),
                    ProductionDate=DateHelpers.SetDateOnlyDaysMonthYear(request.ProductionDate),
                    DeliveryPlace=deliveryPlace,
                    DeliveryPlaceId=deliveryPlace.Id,                  
                };

                _context.Orders.Add(newOrder);
                var groupedPositions = request.OrderPositions.OrderBy(p=>p.Client).ThenBy(p=>p.SetId).ThenBy(p=>p.Lp).GroupBy(p=>p.SetId).ToList();
                var articlesIds = request.OrderPositions.Select(p=>p.ArticleId).Distinct().OrderBy(p=>p).ToList();
                var articles= await _context.Articles.Where(p=>articlesIds.Contains(p.Id)).ToListAsync();
                var positionList = new List<Domain.OrderPosition>();
                foreach(var group in groupedPositions)
                {
                    var set=new Set();
                    _context.Sets.Add(set);
                    var lp=0;
                    foreach(var position in group)
                    {
                        positionList.Add(new Domain.OrderPosition
                        {
                            Order=newOrder,
                            OrderId=newOrder.Id, 
                            ArticleId=position.ArticleId,
                            Article=articles.FirstOrDefault(p=>p.Id==position.ArticleId),
                            Quanity=position.Quanity,
                            Realization=position.Realization,
                            Lp=lp,
                            SetId=set.Id,
                            Set=set,
                            Client=position.Client
                        });
                        lp++;
                    }
                }
                _context.OrderPositions.AddRange(positionList);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create Order");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}