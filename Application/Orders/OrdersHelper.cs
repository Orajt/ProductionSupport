using Domain;

namespace Application.Orders
{
    public class OrdersHelper
    {
        public static CreateNewOrderPositionsResult CreateNewOrderPositionsOrUpdateSetInOldOnes
        (List<OrderPosition.PositionDto> orderPositions, Domain.Order order,
        List<Domain.Article> articles, List<Domain.FabricVariant> variants)
        {
            var groupedPositions = orderPositions
               .OrderBy(p => p.Client).ThenBy(p => p.SetId).ThenBy(p => p.Lp)
               .GroupBy(p => p.SetId)
               .ToList();

            var result = new CreateNewOrderPositionsResult();

            foreach (var group in groupedPositions)
            {
                var set = new Set();
                if(group.First().RelatedSet==null)
                    result.Sets.Add(set);
                else
                    set=group.First().RelatedSet;
                foreach (var position in group)
                {
                    if (position.Id == 0) //that means posision is new
                    {
                        var newPosition = new Domain.OrderPosition
                        {
                            Order = order,
                            OrderId = order.Id,
                            ArticleId = position.ArticleId,
                            Article = articles.FirstOrDefault(p => p.Id == position.ArticleId),
                            Quanity = position.Quanity,
                            Realization = position.Realization,
                            Lp = position.Lp,
                            SetId = set.Id,
                            Set = set,
                            Client = position.Client
                        };

                        foreach (var variant in position.FabricRealization.OrderBy(p => p.PlaceInGroup))
                        {
                            result.OrderPositionRealizations.Add(new OrderPositionRealization
                            {
                                OrderPositionId = newPosition.Id,
                                OrderPosition = newPosition,
                                VarriantId = variant.Id,
                                Variant = variants.FirstOrDefault(p => p.Id == variant.Id),
                                Fabric = articles.FirstOrDefault(p => p.Id == variant.FabricId),
                                FabricId = variant.FabricId,
                                PlaceInGroup = variant.PlaceInGroup
                            });
                        }
                        result.OrderPositions.Add(newPosition);
                        continue;
                    }
                    ///Update set if position is old//
                    position.OrderPositionRefersTo.SetId=set.Id;
                    position.OrderPositionRefersTo.Set=set;
                }
            }
            return result;
        }
        public static void UpdateOrderPosition(Domain.OrderPosition positionInContext, OrderPosition.PositionDto updatedPosition)
        {
            positionInContext.Quanity = updatedPosition.Quanity;
            positionInContext.Lp = updatedPosition.Lp;
            positionInContext.Quanity = updatedPosition.Quanity;
            positionInContext.Client = updatedPosition.Client;
        }
        public static void UpdatePositionSet(ICollection<Domain.OrderPosition> positionsInContext, Domain.OrderPosition choosenPosition, OrderPosition.PositionDto updatedPosition)
        {
            var orderPositionWithSameSetId = positionsInContext.FirstOrDefault(p => p.ArticleId!=updatedPosition.ArticleId && p.SetId == updatedPosition.SetId);
            if (orderPositionWithSameSetId != null)
            {
                choosenPosition.SetId = orderPositionWithSameSetId.SetId;
                choosenPosition.Set = orderPositionWithSameSetId.Set;
                return;
            }
            choosenPosition.SetId = null;
            choosenPosition.Set = null;

            updatedPosition.SetIdFromDB = false;
            updatedPosition.OrderPositionRefersTo = choosenPosition;
        }

        public class CreateNewOrderPositionsResult
        {
            public List<Domain.OrderPosition> OrderPositions { get; set; } = new List<Domain.OrderPosition>();
            public List<Domain.Set> Sets { get; set; } = new List<Domain.Set>();
            public List<OrderPositionRealization> OrderPositionRealizations { get; set; } = new List<OrderPositionRealization>();
        }
    }
}