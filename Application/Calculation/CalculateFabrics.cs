using System.Text;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Calculation
{
    public class CalculateFabrics
    {
        public class Query : IRequest<Result<MyFileResult>>
        {
            public int OrderId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<MyFileResult>>
        {
            private readonly ICalculateFabricsCreatePdf _pdfCreator;
            private readonly IUnitOfWork _unitOfWork;
            public Handler(DataContext context, ICalculateFabricsCreatePdf pdfCreator, IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _pdfCreator = pdfCreator;
            }
            private List<CalculateFabricsPosition> positionList = new List<CalculateFabricsPosition>();
            private List<CalculateFabricsFabrics> fabricList = new List<CalculateFabricsFabrics>();
            private List<CalculateFabricsUnableToFind> unableToFindFabricList = new List<CalculateFabricsUnableToFind>();
            public async Task<Result<MyFileResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = await _unitOfWork.Orders.GetOrderWithArticleDetailsAndPositionRealizations(request.OrderId);

                if (order == null) return null;

                foreach (var position in order.OrderPositions)
                {
                    var positionArticleType = position.Article.ArticleTypeId;
                    ////if article type is not 6 or 1 then article doesnt contains fabric so it could be skipped//////////
                    if (positionArticleType != 6 && positionArticleType != 1)
                    {
                        continue;
                    }
                    ////if article type is 6 then client ordered exactly a fabric so position quanity is fabric quanity///
                    if (positionArticleType == 6)
                    {
                        HandleResultToArticleType6(position);
                        continue;
                    }
                    ///If article type is 1 then fabrics have to be calculated///
                    if (positionArticleType == 1)
                    {
                        var positionRealizationsList = RealizationsGroupedByIdAndCreateCodeForEveryGroup(position);
                        var positionCalculated = true;
                        var calculatedRealization = "";
                        foreach (var realization in positionRealizationsList)
                        {
                            var findedRealization = position.Article.Realizations.FirstOrDefault(p => p.StuffId == realization.StuffId && p.CalculatedCode == realization.Code);
                            if (findedRealization == null)
                            {
                                positionCalculated = false;
                                unableToFindFabricList.Add(new CalculateFabricsUnableToFind
                                {
                                    PositionId = position.Id,
                                    ArticleName = position.Article.FullName,
                                    Code = realization.Code,
                                    StuffName = realization.StuffName
                                });
                            }
                            if (findedRealization != null)
                            {
                                calculatedRealization += realization.Variants + ": " + realization.FabricName + " - "
                                    + findedRealization.FabricLength + "m; ";
                                var fabricExistsInList = fabricList.FirstOrDefault(p => p.FabricId == realization.FabricId);
                                if (fabricExistsInList != null)
                                {
                                    fabricExistsInList.Quanity += position.Quanity * findedRealization.FabricLength;
                                    continue;
                                }
                                fabricList.Add(new CalculateFabricsFabrics
                                {
                                    FabricId = realization.FabricId,
                                    FabricName = realization.FabricName,
                                    Quanity = position.Quanity * findedRealization.FabricLength,
                                    Company = ""
                                });
                            }
                        }
                        if (positionCalculated)
                        {
                            position.CalculatedRealization = calculatedRealization;
                        }
                        positionList.Add(new CalculateFabricsPosition
                        {
                            OrderPositionId = position.Id,
                            Article = position.Article.FullName,
                            CalculatedRealization = calculatedRealization,
                            FabricsCalculated = positionCalculated,
                            Quanity = position.Quanity,
                            Client = position.Client
                        });
                    }
                }

                var pdfFile = _pdfCreator.CreateMainPdf(positionList, fabricList, unableToFindFabricList, order.Name);

                var result = await _unitOfWork.SaveChangesAsync();

                return Result<MyFileResult>.Success(new MyFileResult() { FileName = $"{order.Name}_fabrics", File = pdfFile });
            }
            private void HandleResultToArticleType6(Domain.OrderPosition position)
            {
                bool fabricsCalculated = false;
                var fabricInList = fabricList.FirstOrDefault(p => p.FabricId == position.ArticleId);
                if (fabricInList == null)
                {
                    fabricList.Add(new CalculateFabricsFabrics
                    {
                        FabricId = position.ArticleId,
                        FabricName = position.Article.FullName,
                        Quanity = position.Quanity,
                        Company = ""
                    });
                    fabricsCalculated = true;
                }
                if (fabricsCalculated == false)
                {
                    fabricInList.Quanity += position.Quanity;
                }
                positionList.Add(new CalculateFabricsPosition
                {
                    OrderPositionId = position.Id,
                    Article = position.Article.FullName,
                    CalculatedRealization = $"{position.Article.FullName}: {position.Quanity}",
                    FabricsCalculated = fabricsCalculated,
                    Quanity = position.Quanity,
                    Client = position.Client
                });
            }
            private List<CodeStuffResult> RealizationsGroupedByIdAndCreateCodeForEveryGroup(Domain.OrderPosition position)
            {
                var fvgCount = position.Article.FabricVariant.FabricVariants.Count();
                //Group position realization by fabric id to calculate code which is necessary to
                //find correct fabric length
                var positionRealizationGrouped = position.Realizations
                    .OrderBy(p => p.PlaceInGroup)
                    .GroupBy(p => p.FabricId)
                    .ToList();

                var positionRealizationsList = new List<CodeStuffResult>();
                foreach (var group in positionRealizationGrouped)
                {
                    var newPositionRealization = new CodeStuffResult()
                    {
                        StuffId = (int)group.First().Fabric.StuffId,
                        StuffName = group.First().Fabric.Stuff.Name,
                        FabricId = group.Key,
                        FabricName = group.First().Fabric.FullName
                    };
                    var code = "";
                    ///calculate code to group//
                    for (int i = 0; i < fvgCount; i++)
                    {
                        code += 0;
                    }
                    var placesOfVariant = group.Select(p => p.PlaceInGroup).ToList();
                    StringBuilder str = new StringBuilder(code);
                    bool firstVariant = true;
                    foreach (var member in group)
                    {
                        str[member.PlaceInGroup - 1] = '1';
                        if (firstVariant)
                        {
                            newPositionRealization.Variants += member.Variant.ShortName;
                            firstVariant = false;
                            continue;
                        }
                        newPositionRealization.Variants += "+" + member.Variant.ShortName;

                    }
                    newPositionRealization.Code = str.ToString();
                    positionRealizationsList.Add(newPositionRealization);
                }
                return positionRealizationsList;
            }
        }
    }
}
