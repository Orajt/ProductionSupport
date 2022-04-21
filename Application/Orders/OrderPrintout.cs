using Application.Core;
using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.AspNetCore.Hosting;

namespace Application.Orders
{
    public class OrderPrintout
    {
        public class Query : IRequest<Result<MyFileResult>>
        {
            public int OrderId { get; set; }
            public int ArticleTypeId { get; set; }
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
                        .ThenInclude(p => p.Article)
                            .ThenInclude(p => p.Stuff)
                    .Include(p => p.OrderPositions)
                        .ThenInclude(p => p.Article)
                            .ThenInclude(p => p.FilePaths)
                    .FirstOrDefaultAsync(p => p.Id == request.OrderId);

                if (order == null) return null;

                if (order.OrderPositions.Count == 0)
                    return Result<MyFileResult>.Failure("Order doesn't contains positions");

                var articleTypesToGoThrough = new List<int>();

                var articleType = await _context.ArticleTypes.FirstOrDefaultAsync(p => p.Id == request.ArticleTypeId);

                if (articleType == null) return null;

                Core.Relations.FindEveryParentToArticleType(articleTypesToGoThrough, request.ArticleTypeId);

                articleTypesToGoThrough.Add(request.ArticleTypeId);

                var articleList = new List<OrderPrintoutDto>();

                foreach (var orderPosition in order.OrderPositions)
                {
                    if (orderPosition.Article.ArticleTypeId == request.ArticleTypeId)
                    {
                        var existingArticle = articleList.FirstOrDefault(p => p.ArticleId == orderPosition.ArticleId);
                        if (existingArticle != null)
                        {
                            existingArticle.Quanity += orderPosition.Quanity;
                            continue;
                        }

                        var newItem = new OrderPrintoutDto()
                        {
                            ArticleId = orderPosition.ArticleId,
                            ArticleName = orderPosition.Article.FullName,
                            Quanity = orderPosition.Quanity,
                            StuffId = orderPosition.Article.StuffId != null ? (int)orderPosition.Article.StuffId : 0,
                            Stuff = orderPosition.Article.Stuff != null ? orderPosition.Article.Stuff.Name : "none",
                            Width = orderPosition.Article.Width,
                            Length = orderPosition.Article.Length,
                            Height = orderPosition.Article.High
                        };
                        var pdfFilePath = orderPosition.Article.FilePaths.FirstOrDefault(p => p.FileType == "pdf");
                        if (pdfFilePath != null)
                            newItem.PdfId = pdfFilePath.Id;
                        articleList.Add(newItem);

                        continue;
                    }
                    Core.TreeHelpers.CalculateArticlesBasedOnArticleType(articleList, orderPosition.ArticleId, orderPosition.Quanity, orderPosition.Article.FullName, orderPosition.Quanity, request.ArticleTypeId, _context, articleTypesToGoThrough);
                }

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
                List<Image> images = new List<Image>();
                ///////Font///////////////

                PdfFont normalFont = PdfFontFactory.CreateFont(_env.WebRootPath + $"\\fonts\\" + "NotoSans-Light.ttf", "Identity-H", pdf);

                var groupedArticles = articleList.GroupBy(p => p.StuffId).ToList();

                foreach (var group in groupedArticles)
                {
                    //////////Group title///////////
                    var data = DateTime.Now;
                    Paragraph subheader = new Paragraph($"Printed: {data.Day}-{data.Month}-{data.Year}")
                    .SetFont(normalFont)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(12);

                    var stuffName = group.First().Stuff;
                    if(request.ArticleTypeId!=4)
                    {
                        stuffName="";
                    }
                    Paragraph header = new Paragraph($"{stuffName} Order: {order.Name}")
                     .SetFont(normalFont)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFontSize(18);

                    document.Add(subheader);
                    document.Add(header);
                    document.Add(newLine);
                    document.Add(ls);

                    ///////Create group table//////////
                    tabels.Add(new Table(20, true));
                    var actualTab = tabels[tabels.Count - 1];
                    

                    /////////Table header///////////
                    cells.Add(Core.PdfElements.CreateHeaderCell("PDF FILE", 1, 4, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateHeaderCell("STUFF", 1, 3, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateHeaderCell("NAME", 1, 6, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateHeaderCell("QUA", 1, 1, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);
                    cells.Add(Core.PdfElements.CreateHeaderCell("PARENTS", 1, 6, normalFont));
                    actualTab.AddCell(cells[cells.Count - 1]);

                     document.Add(actualTab);
                    //////////Table rows////////////////
                    foreach (var article in group.OrderBy(p => p.Width).ThenBy(p => p.Height).ThenBy(p => p.Length).ThenBy(p => p.ArticleName))
                    {
                        if (article.PdfId != 0)
                        {
                            images.Add(Core.PdfElements.CreateQrCodeImage(article.PdfId));
                            cells.Add(Core.PdfElements.CreateImageCell(images[images.Count - 1], 1, 4));
                        }
                        if (article.PdfId == 0)
                            cells.Add(Core.PdfElements.CreateStandardCell("None", 1, 4, normalFont));
                        actualTab.AddCell(cells[cells.Count - 1]);
                        cells.Add(Core.PdfElements.CreateStandardCell(article.Stuff, 1, 3, normalFont));
                        actualTab.AddCell(cells[cells.Count - 1]);
                        cells.Add(Core.PdfElements.CreateStandardCell(article.ArticleName, 1, 6, normalFont));
                        actualTab.AddCell(cells[cells.Count - 1]);
                        cells.Add(Core.PdfElements.CreateStandardCell(article.Quanity.ToString(), 1, 1, normalFont));
                        actualTab.AddCell(cells[cells.Count - 1]);
                        var parents = "";
                        foreach (var parent in article.ParentAndCount)
                            parents += parent + "; ";
                        cells.Add(Core.PdfElements.CreateStandardCell(parents, 1, 6, normalFont));
                        actualTab.AddCell(cells[cells.Count - 1]);
                    }
                    document.Add(actualTab);
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }

                document.Close();
                result = stream.ToArray();

                return Result<MyFileResult>.Success(new MyFileResult { FileName = $"{order.Name}_artType_{articleType.Name}", File = result });

            }
        }
    }
}