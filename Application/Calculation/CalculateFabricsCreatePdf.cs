using Application.Core;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;

namespace Application.Calculation
{
    public interface ICalculateFabricsCreatePdf
    {
        byte[] CreateMainPdf(List<CalculateFabricsPosition> positionList, List<CalculateFabricsFabrics> fabricList, List<CalculateFabricsUnableToFind> unableToFindFabricList, string orderName);
    }

    public class CalculateFabricsCreatePdf : ICalculateFabricsCreatePdf
    {
        private readonly IWebHostEnvironment _env;
        public CalculateFabricsCreatePdf(IWebHostEnvironment env)
        {
            _env = env;
        }
        public byte[] CreateMainPdf(List<CalculateFabricsPosition> positionList, List<CalculateFabricsFabrics> fabricList, List<CalculateFabricsUnableToFind> unableToFindFabricList, string orderName)
        {
            //////Order lists/////
            fabricList = fabricList.OrderBy(p => p.FabricName).ToList();
            unableToFindFabricList = unableToFindFabricList.OrderBy(p => p.ArticleName).ThenBy(p => p.StuffName).ThenBy(p => p.Code).ToList();
            positionList = positionList.OrderBy(p => p.Client).ToList();

            ///////Create pdf file////////
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

            string fontPath = Path.Combine(_env.WebRootPath, "fonts", "NotoSans-Light.ttf");
            PdfFont normalFont = PdfFontFactory.CreateFont(fontPath, "Identity-H", pdf);

            var date = DateTime.Now;

            //Unable to find fabrics table//
            document = AddHeader(document, orderName, normalFont);
            document = CreateUnableToFindFabricTable(unableToFindFabricList, document, normalFont);

            //Fabric table//
            document = AddHeader(document, orderName, normalFont);
            document = CreateFabricTable(fabricList, document, normalFont);

            //Order position table//
            document = AddHeader(document, orderName, normalFont);
            document = CreatePositionsTable(positionList, document, normalFont);

            document.Close();
            return stream.ToArray();

        }
        private Document AddHeader(Document document, string OrderName, PdfFont font)
        {
            var date = DateTime.Now;
            var commonPdfElements = new CommonPdfElements();

            Paragraph header = new Paragraph($"Order: {OrderName}")
                 .SetFont(font)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFontSize(18);

            Paragraph subheader = new Paragraph($"Printed: {date.Day}-{date.Month}-{date.Year}")
                .SetFont(font)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(12);

            document.Add(header);
            document.Add(subheader);
            document.Add(commonPdfElements.NewLine);
            document.Add(commonPdfElements.Ls);

            return document;
        }
        private Document CreateUnableToFindFabricTable(List<CalculateFabricsUnableToFind> unableToFindFabricList, Document document, PdfFont font)
        {
            var cells = new List<Cell>();
            var table = new Table(20, true);

            Paragraph subheaderUnableToFind = new Paragraph($"List of fabric realizations that couldn't be found, should be empty if all positions were calculated properly:")
                 .SetFont(font)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFontSize(12);

            document.Add(subheaderUnableToFind);

            cells.Add(Core.PdfElements.CreateHeaderCell("Position Id", 1, 4, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Article name", 1, 6, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Stuff Name", 1, 5, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Code", 1, 5, font));
            table.AddCell(cells[cells.Count - 1]);
            foreach (var utf in unableToFindFabricList)
            {
                cells.Add(Core.PdfElements.CreateStandardCell(utf.PositionId.ToString(), 1, 4, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(utf.ArticleName, 1, 6, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(utf.StuffName, 1, 5, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(utf.Code, 1, 5, font));
                table.AddCell(cells[cells.Count - 1]);
            }
            document.Add(table);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            return document;
        }
        private Document CreateFabricTable(List<CalculateFabricsFabrics> fabricList, Document document, PdfFont font)
        {
            var cells = new List<Cell>();
            var table = new Table(20, true);

            Paragraph subHeaderFabricList = new Paragraph($"List of fabrics necesary to make order:")
                .SetFont(font)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12);

            document.Add(subHeaderFabricList);

            cells.Add(Core.PdfElements.CreateHeaderCell("Fabric id", 1, 4, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Fabric name", 1, 6, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Company", 1, 5, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Quanity", 1, 5, font));
            table.AddCell(cells[cells.Count - 1]);

            foreach (var fabric in fabricList)
            {
                cells.Add(Core.PdfElements.CreateStandardCell(fabric.FabricId.ToString(), 1, 4, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(fabric.FabricName, 1, 6, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell("Have to implement that", 1, 5, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(fabric.Quanity.ToString("0.00"), 1, 5, font));
                table.AddCell(cells[cells.Count - 1]);
            }
            document.Add(table);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            return document;
        }
        private Document CreatePositionsTable(List<CalculateFabricsPosition> positionList, Document document, PdfFont font)
        {
            var cells = new List<Cell>();
            var table = new Table(20, true);
            Paragraph subHeaderPositions = new Paragraph($"List of articles that includes fabrics:")
                .SetFont(font)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12);

            document.Add(subHeaderPositions);

            cells.Add(Core.PdfElements.CreateHeaderCell("Position id", 1, 2, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Article", 1, 4, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Calculated Realization", 1, 5, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Quanity", 1, 3, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Fabrics calculated", 1, 2, font));
            table.AddCell(cells[cells.Count - 1]);
            cells.Add(Core.PdfElements.CreateHeaderCell("Client", 1, 4, font));
            table.AddCell(cells[cells.Count - 1]);

            foreach (var pos in positionList)
            {
                cells.Add(Core.PdfElements.CreateStandardCell(pos.OrderPositionId.ToString(), 1, 2, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(pos.Article, 1, 4, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(pos.CalculatedRealization, 1, 5, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(pos.Quanity.ToString(), 1, 3, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(pos.FabricsCalculated.ToString(), 1, 2, font));
                table.AddCell(cells[cells.Count - 1]);
                cells.Add(Core.PdfElements.CreateStandardCell(pos.Client, 1, 4, font));
                table.AddCell(cells[cells.Count - 1]);
            }
            document.Add(table);
            return document;
        }


    }
}