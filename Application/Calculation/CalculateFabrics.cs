using System.Text;
using Application.Core;
using Application.Orders;
using AutoMapper;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            private readonly DataContext _context;
            private readonly IWebHostEnvironment _env;
            public Handler(DataContext context, IWebHostEnvironment env)
            {
                _env = env;
                _context = context;
            }

            public async Task<Result<MyFileResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders
                    .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Realizations)
                            .ThenInclude(p => p.Variant)
                     .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Realizations)
                            .ThenInclude(p => p.Fabric)
                                .ThenInclude(p => p.Stuff)
                    .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Article)
                            .ThenInclude(p => p.FabricVariant)
                                .ThenInclude(p => p.FabricVariants)
                    .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Article)
                             .ThenInclude(p => p.Realizations)
                        .FirstOrDefaultAsync(p => p.Id == request.OrderId);

                var positionList = new List<CalculateFabricsPosition>();
                var fabricList = new List<CalculateFabricsFabrics>();
                var unableToFindFabricList = new List<CalculateFabricsUnableToFind>();

                foreach (var position in order.OrderPositions)
                {
                    var positionArticleType = position.Article.ArticleTypeId;
                    var fabricsCalculated = false;
                    ////if article type is not 6 or 1 then article doesnt contains fabric so it could be skipped//////////
                    if (positionArticleType != 6 && positionArticleType != 1)
                    {
                        continue;
                    }
                    ////if article type is 6 then client ordered exactly a fabric so position quanity is fabric quanity///
                    if (positionArticleType == 6)
                    {
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
                            fabricsCalculated = true;
                        }
                        positionList.Add(new CalculateFabricsPosition
                        {
                            OrderPositionId = position.Id,
                            Article = position.Article.FullName,
                            CalculatedRealization = $"{position.Article.FullName}: {position.Quanity}",
                            FabricsCalculated = fabricsCalculated,
                            Quanity=position.Quanity,
                            Client = position.Client
                        });
                        continue;
                    }
                    ///If article type is 1 then fabric has to be calculated///
                    if (positionArticleType == 1)
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
                                    firstVariant=false;
                                    continue;
                                }
                                newPositionRealization.Variants += "+" + member.Variant.ShortName;

                            }
                            newPositionRealization.Code = str.ToString();
                            positionRealizationsList.Add(newPositionRealization);
                        }

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
                                calculatedRealization += realization.Variants + ": "+ realization.FabricName + " - "
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
                            Quanity=position.Quanity,
                            Client = position.Client
                        });
                    }
                }
                //////Order lists/////
                fabricList = fabricList.OrderBy(p => p.FabricName).ToList();
                unableToFindFabricList = unableToFindFabricList.OrderBy(p => p.ArticleName).ThenBy(p => p.StuffName).ThenBy(p => p.Code).ToList();
                positionList = positionList.OrderBy(p => p.Client).ToList();


                ///////Create pdf file////////
                byte[] result;
                MemoryStream stream = new MemoryStream();
                PdfWriter writer = new PdfWriter(stream);
                writer.SetCloseStream(false);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                //////Pdf elements////////////
                Paragraph newLine = new Paragraph(new Text("\n"));
                LineSeparator ls = new LineSeparator(new SolidLine());
                List<Cell> cells = new List<Cell>();
                List<Table> tabels = new List<Table>();

                PdfFont normalFont = PdfFontFactory.CreateFont(_env.WebRootPath + $"\\fonts\\" + "NotoSans-Light.ttf", "Identity-H", pdf);
                var date = DateTime.Now;

                 /////////////Unable to find list //////////////////
                Paragraph header = new Paragraph($"Order: {order.Name}")
                     .SetFont(normalFont)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFontSize(18);

                Paragraph subheader = new Paragraph($"Printed: {date.Day}-{date.Month}-{date.Year}")
                    .SetFont(normalFont)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(12);

                document.Add(header);
                document.Add(subheader);
                document.Add(newLine);
                document.Add(ls);
               
                Paragraph subheaderUnableToFind = new Paragraph($"List of fabric realizations that couldn't be found, should be empty if all positions were calculated properly:")
                    .SetFont(normalFont)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12);

                document.Add(subheaderUnableToFind);

                tabels.Add(new Table(20, true));
                var actualTab = tabels[tabels.Count - 1];
                cells.Add(Core.PdfElements.CreateHeaderCell("Position Id", 1, 4, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Article name", 1, 6, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Stuff Name", 1, 5, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Code", 1, 5, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                foreach (var utf in unableToFindFabricList)
                {
                    cells.Add(Core.PdfElements.CreateStandardCell(utf.PositionId.ToString(), 1, 4, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(utf.ArticleName, 1, 6, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(utf.StuffName, 1, 5, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(utf.Code, 1, 5, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]); 
                }
                document.Add(actualTab);
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                /////////////Fabric list///////////////
                document.Add(header);
                document.Add(subheader);
                document.Add(newLine);
                document.Add(ls);

                Paragraph subHeaderFabricList = new Paragraph($"List of fabrics necesary to make order:")
                    .SetFont(normalFont)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12);

                document.Add(subHeaderFabricList);

                tabels.Add(new Table(20, true));
                actualTab = tabels[tabels.Count - 1];
                cells.Add(Core.PdfElements.CreateHeaderCell("Fabric id", 1, 4, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Fabric name", 1, 6, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Company", 1, 5, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Quanity", 1, 5, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);

                foreach(var fabric in fabricList)
                {
                    cells.Add(Core.PdfElements.CreateStandardCell(fabric.FabricId.ToString(), 1, 4, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(fabric.FabricName, 1, 6, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell("Have to implement that", 1, 5, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(fabric.Quanity.ToString("0.00"), 1, 5, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]); 
                }
                document.Add(actualTab);
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                
                /////////////Order position list///////////////
                document.Add(header);
                document.Add(subheader);
                document.Add(newLine);
                document.Add(ls);

                Paragraph subHeaderPositions = new Paragraph($"List of articles that includes fabrics:")
                    .SetFont(normalFont)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12);

                document.Add(subHeaderPositions);


                tabels.Add(new Table(20, true));
                actualTab = tabels[tabels.Count - 1];
                cells.Add(Core.PdfElements.CreateHeaderCell("Position id", 1, 2, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Article", 1, 4, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Calculated Realization", 1, 5, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Quanity", 1, 3, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Fabrics calculated", 1, 2, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateHeaderCell("Client", 1, 4, normalFont));
                actualTab.AddCell(cells[cells.Count - 1]);

                foreach(var pos in positionList)
                {
                    cells.Add(Core.PdfElements.CreateStandardCell(pos.OrderPositionId.ToString(), 1, 2, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(pos.Article, 1, 4, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(pos.CalculatedRealization, 1, 5, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                     cells.Add(Core.PdfElements.CreateStandardCell(pos.Quanity.ToString(), 1, 3, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateStandardCell(pos.FabricsCalculated.ToString(), 1, 2, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]); 
                    cells.Add(Core.PdfElements.CreateStandardCell(pos.Client, 1, 4, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                }
                document.Add(actualTab);
                document.Close();
                result = stream.ToArray();
                if(!unableToFindFabricList.Any())
                    order.FabricsCalculated=true;

                _context.SaveChanges();
                return Result<MyFileResult>.Success(new MyFileResult(){ FileName = $"{order.Name}_fabrics", File = result });
            }
        }
    }
}