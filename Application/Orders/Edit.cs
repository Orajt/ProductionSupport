using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Orders
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int DeliveryPlaceId { get; set; }
            public DateTime ShipmentDate { get; set; }
            public DateTime ProductionDate { get; set; }
            public List<OrderPosition.PositionDto> OrderPositions{get;set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Name).NotNull();
                RuleFor(p => p.DeliveryPlaceId).NotNull();

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
                

                var order = await _context.Orders.Include(p=>p.OrderPositions).FirstOrDefaultAsync(p => p.Id == request.Id);
                if(order==null) return null;

                if (request.Name.ToUpper()!=order.Name.ToUpper() &&  await _context.Orders.AsNoTracking()
                    .AnyAsync(p => p.Name.ToUpper() == request.Name.ToUpper()))
                    return Result<Unit>.Failure($"Order named {request.Name} exist in database");

                if (order.DeliveryPlaceId != request.DeliveryPlaceId){
                    var deliveryPlace = await _context.DeliveryPlaces.FirstOrDefaultAsync(p => p.Id == request.DeliveryPlaceId);
                    if (deliveryPlace==null) return null;
                    order.DeliveryPlace = deliveryPlace;
                    order.DeliveryPlaceId = deliveryPlace.Id;
                }

                order.Name = request.Name;
                order.EditDate = DateTime.Now;
                order.ShipmentDate = request.ShipmentDate;
                order.ProductionDate = request.ProductionDate;

                var requestPositionsInDB = request.OrderPositions.Where(p=>p.Id!=0).ToList();
                var requestPositionsNew = request.OrderPositions.Where(p=>p.Id==0).ToList();

                var requestPositionIds=requestPositionsInDB.Select(p=>p.Id).Distinct().ToList();

                _context.RemoveRange(order.OrderPositions.Where(p=>!requestPositionIds.Contains(p.Id)));

                var setList = new List<Domain.Set>();

                foreach(var position in requestPositionsInDB)
                {
                    var choosenPosition = order.OrderPositions.FirstOrDefault(p=>p.Id==position.Id);
                    if(choosenPosition==null){
                        continue;
                    }
                    choosenPosition.Quanity=position.Quanity;
                    choosenPosition.Realization=position.Realization;
                    choosenPosition.Lp=position.Lp;
                    choosenPosition.Quanity=position.Quanity;
                    choosenPosition.Client=position.Client;
                    var orderPositionWithSameSetId=order.OrderPositions.FirstOrDefault(p=>p.SetId==position.SetId);
                    if(orderPositionWithSameSetId!=null)
                    {
                        choosenPosition.SetId=orderPositionWithSameSetId.SetId;
                        choosenPosition.Set=orderPositionWithSameSetId.Set;
                    }
                    else{
                        var listOfSameId=requestPositionsNew.Where(p=>p.SetId==position.SetId).ToList();
                        if(listOfSameId.Count==0)
                        {
                            choosenPosition.SetId=null;
                            choosenPosition.Set=null;
                            continue;
                        }
                        else{
                            var set=new Set();
                            _context.Add(set);
                            setList.Add(set);
                            choosenPosition.SetId=set.Id;
                            choosenPosition.Set=set;
                            for(int i=0;i<listOfSameId.Count;i++)
                            {
                                var positionToChaange=requestPositionsNew.FirstOrDefault(p=>p==listOfSameId[i]);
                                positionToChaange.SetId=set.Id;
                                positionToChaange.SetIdFromDB=true;
                                positionToChaange.IndexOfSetList=setList.Count-1;
                            }
                        }
                    }
                }
                var usedArticles= requestPositionsNew.Select(p=>p.ArticleId).ToList();
                var groupedPositions = requestPositionsNew.OrderBy(p=>p.Client).ThenBy(p=>p.SetId).ThenBy(p=>p.Lp).GroupBy(p=>p.SetId).ToList();

                var articles = await _context.Articles.Where(p=>usedArticles.Contains(p.Id)).ToListAsync();

                var newPositionList = new List<Domain.OrderPosition>();

                foreach(var group in groupedPositions)
                {
                    var set=new Set();
                    var setIdDB=group.FirstOrDefault(p=>p.SetIdFromDB==true);

                    if(setIdDB!=null && setIdDB.IndexOfSetList!=null)
                    {
                        set=setList[(int)setIdDB.IndexOfSetList];
                    }
                    else{
                        _context.Add(set);
                    }
                   
                    foreach(var position in group)
                    {
                        var article = articles.FirstOrDefault(p=>p.Id==position.ArticleId);
                        if(article==null) return null;
                        newPositionList.Add(new Domain.OrderPosition
                        {
                            Order=order,
                            OrderId=order.Id, 
                            ArticleId=position.ArticleId,
                            Article=article,
                            Quanity=position.Quanity,
                            Realization=position.Realization,
                            Lp=position.Lp,
                            SetId=set.Id,
                            Set=set,
                            Client=position.Client
                        });
                    }
                }
                var orderPositionAsList = order.OrderPositions.ToList();
                orderPositionAsList.AddRange(newPositionList);
                order.OrderPositions=orderPositionAsList;
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit Order");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}