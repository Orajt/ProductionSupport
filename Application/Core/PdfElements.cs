using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Drawing;
using QRCoder;


namespace Application.Core
{
    public static class PdfElements
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
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(2);
            return BitmapToBytes(qrCodeImage);
        }
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        // public static Bitmap ResizeImage(Image image, int width, int height)
        // {
        //     var destRect = new Rectangle(0, 0, width, height);
        //     var destImage = new Bitmap(width, height);

        //     destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //     using (var graphics = Graphics.FromImage(destImage))
        //     {
        //         graphics.CompositingMode = CompositingMode.SourceCopy;
        //         graphics.CompositingQuality = CompositingQuality.HighQuality;
        //         graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //         graphics.SmoothingMode = SmoothingMode.HighQuality;
        //         graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //         using (var wrapMode = new ImageAttributes())
        //         {
        //             wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //             graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //         }
        //     }

        //     return destImage;

        // }
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
            var text=Core.HardcodedClientAppUrls.PdfViewUrl(id);

            var newImage = new iText.Layout.Element.Image(ImageDataFactory
                                .Create(CreateQrCode(text)));
            newImage.SetTextAlignment(TextAlignment.CENTER);

            return newImage;
        }


    }
}