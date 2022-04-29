using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;

namespace Application.Core
{
    public class PdfElements
    {
        public static Cell CreateHeaderCell(string content, int rowSpan, int colSpan, PdfFont font)
        {
            return new Cell(rowSpan, colSpan)
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                          .SetFont(font)
                          .SetFontSize(9)
                          .SetBold()
                          .SetTextAlignment(TextAlignment.CENTER)
                          .Add(new Paragraph(content));
        }
        public static Byte[] CreateQrCode(string url)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url,
            QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(2);
            return qrCodeImage;
        }
        public static Cell CreateStandardCell(string content, int rowSpan, int colSpan, PdfFont font)
        {
            return new Cell(rowSpan, colSpan)
                        .SetFont(font)
                        .SetFontSize(9)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .Add(new Paragraph(content));
        }
        public static Cell CreateImageCell(iText.Layout.Element.Image content, int rowSpan, int colSpan)
        {
            return new Cell(rowSpan, colSpan)
                        .Add(content);
        }
        public static iText.Layout.Element.Image CreateQrCodeImage(int id)
        {
            var text = Core.HardcodedClientAppUrls.PdfViewUrl(id);

            var newImage = new iText.Layout.Element.Image(ImageDataFactory
                                .Create(CreateQrCode(text)));
            newImage.SetTextAlignment(TextAlignment.CENTER);

            return newImage;
        }

    }
    public class CommonPdfElements
    {
        public Paragraph NewLine { get; set; } = new Paragraph(new Text("\n"));
        public LineSeparator Ls { get; set; } = new LineSeparator(new SolidLine());
        public AreaBreak BreakPage { get; set; } = new AreaBreak(AreaBreakType.NEXT_PAGE);
    }
}
