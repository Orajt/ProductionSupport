using Application.Core;
using Application.Interfaces;
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
            public List<OrderPosition.PositionDto> OrderPositions { get; set; }
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
            private readonly IUnitOfWork _unitOfWork;
            public Handler(DataContext context, IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                var order = await _unitOfWork.Orders.GetOrderWithOrderPositions(request.Id);
                if (order == null) return null;

                if (request.Name.ToUpper() != order.Name.ToUpper() && await _unitOfWork.Orders.IsOrderNameTaken(request.Name))
                    return Result<Unit>.Failure($"Order named {request.Name} exist in database");

                if (order.DeliveryPlaceId != request.DeliveryPlaceId)
                {
                    var deliveryPlace = await _unitOfWork.DeliveryPlaces.Find(request.DeliveryPlaceId);
                    if (deliveryPlace == null) return null;

                    order.DeliveryPlace = deliveryPlace;
                    order.DeliveryPlaceId = deliveryPlace.Id;
                }

                order.Name = request.Name;
                order.EditDate = DateHelpers.SetDateTimeToCurrent(DateTime.Now).Date;
                order.ShipmentDate = request.ShipmentDate.Date;
                order.ProductionDate = request.ProductionDate.Date;

                var requestPositionsInDB = request.OrderPositions.Where(p => p.Id != 0).ToList();
                var requestPositionsNew = request.OrderPositions.Where(p => p.Id == 0).ToList();

                var requestPositionsInDBIds = requestPositionsInDB.Select(p => p.Id).ToList();

                var orderPositionsToRemove = order.OrderPositions.Where(p => !requestPositionsInDBIds.Contains(p.Id)).ToList();

                _unitOfWork.OrderPositions.RemoveRange(orderPositionsToRemove);

                var setList = new List<Domain.Set>();

                foreach (var position in requestPositionsInDB)
                {
                    var choosenPosition = order.OrderPositions.FirstOrDefault(p => p.Id == position.Id);
                    if (choosenPosition == null)
                    {
                        continue;
                    }
                    OrdersHelper.UpdateOrderPosition(choosenPosition, position);
                    OrdersHelper.UpdatePositionSet(order.OrderPositions, choosenPosition, position);
                }
                var usedArticles = requestPositionsNew.Select(p => p.ArticleId).Distinct().ToList();
                var usedFabrics = request.OrderPositions.SelectMany(p => p.FabricRealization).Select(p => p.FabricId).Distinct().ToList();
                usedArticles.AddRange(usedFabrics);
                var articles = await _unitOfWork.Articles.GetArticlesWithFabricVariantsBasedOnArtclesIds(usedArticles);
                if (articles.Count != usedArticles.Count) return null;

                var usedVariants = request.OrderPositions.SelectMany(p => p.FabricRealization).Select(p => p.Id).Distinct().ToList();
                var variants = await _unitOfWork.FabricVariants.Where(p => usedVariants.Contains(p.Id));
                if (usedVariants.Count != variants.Count) return null;

                foreach(var position in requestPositionsNew)
                {
                    var relatedSet = order.OrderPositions.FirstOrDefault(p=>p.SetId==position.SetId);
                    if(relatedSet!=null) position.RelatedSet=relatedSet.Set;
                }

                requestPositionsNew.AddRange(requestPositionsInDB.Where(p => p.SetIdFromDB == false).ToList());

                var newPositions = OrdersHelper.CreateNewOrderPositionsOrUpdateSetInOldOnes(requestPositionsNew, order, articles, variants);

                order.FabricsCalculated = false;
                order.Done = false;

                _unitOfWork.Sets.AddRange(newPositions.Sets);
                _unitOfWork.OrderPositions.AddRange(newPositions.OrderPositions);
                _unitOfWork.OrderPositionsRealizations.AddRange(newPositions.OrderPositionRealizations);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to edit Order");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}