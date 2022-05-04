using Application.Core;
using Application.Interfaces;
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
            public DateTime ProductionDate { get; set; }
            public List<OrderPosition.PositionDto> OrderPositions { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Name).NotNull();
                RuleFor(p => p.DeliveryPlaceId).NotNull();
                RuleFor(p => p.ShipmentDate).NotNull();
                RuleFor(p => p.ProductionDate).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                if (await _unitOfWork.Orders.IsOrderNameTaken(request.Name))
                    return Result<Unit>.Failure($"Order named {request.Name} exist in database");

                var deliveryPlace = await _unitOfWork.DeliveryPlaces.Find(request.DeliveryPlaceId);
                if (deliveryPlace == null) return null;

                var newOrder = new Domain.Order
                {
                    Name = request.Name,
                    EditDate = DateHelpers.SetDateTimeToCurrent(DateTime.Now).Date,
                    ShipmentDate = DateHelpers.SetDateTimeToCurrent(request.ShipmentDate).Date,
                    ProductionDate = DateHelpers.SetDateTimeToCurrent(request.ProductionDate).Date,
                    DeliveryPlace = deliveryPlace,
                    DeliveryPlaceId = deliveryPlace.Id,
                };
                _unitOfWork.Orders.Add(newOrder);

                //Manage order positions//
                var articlesIds = request.OrderPositions.Select(p => p.ArticleId).Distinct().OrderBy(p => p).ToList();
                var usedFabrics = request.OrderPositions.SelectMany(p => p.FabricRealization).Select(p => p.FabricId).Distinct().ToList();
                articlesIds.AddRange(usedFabrics);
                var articles = await _unitOfWork.Articles.GetArticlesWithFabricVariantsBasedOnArtclesIds(articlesIds);

                var usedVariants = request.OrderPositions.SelectMany(p => p.FabricRealization).Select(p => p.Id).Distinct().ToList();
                var variants = await _unitOfWork.FabricVariants.Where(p => usedVariants.Contains(p.Id));

                var newPositions = OrdersHelper.CreateNewOrderPositionsOrUpdateSetInOldOnes(request.OrderPositions, newOrder, articles, variants);

                //Add data to context//
                _unitOfWork.Sets.AddRange(newPositions.Sets);
                _unitOfWork.OrderPositions.AddRange(newPositions.OrderPositions);
                _unitOfWork.OrderPositionsRealizations.AddRange(newPositions.OrderPositionRealizations);

                var result = await _unitOfWork.SaveChangesAsync();

                if (!result) return Result<Unit>.Failure("Failed to create Order");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}