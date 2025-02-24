using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.Layout.Properties;
using iText.IO.Image;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using SkiaSharp;
using iText.Layout.Renderer;
using ZXing.SkiaSharp.Rendering;
using iText.Kernel.Geom;

namespace Entree_1830974.Models
{
    public class BarcodePdfGenerator
    {
        public static void GenerateTicketPdf(Ticket ticket, string pdfPath)
        {
            try
            {
                // Barcode Generation
                BarcodeWriter<SKBitmap> barcodeWriter = new BarcodeWriter<SKBitmap>
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions { Width = 300, Height = 100, Margin = 10 },
                    Renderer = new SKBitmapRenderer()
                };
                SKBitmap skBitmap = barcodeWriter.Write(ticket.Id.ToString());

                // Save barcode to a temporary file
                string tempBarcodePath = System.IO.Path.GetTempFileName() + ".png";
                using (SKImage image = SKImage.FromBitmap(skBitmap))
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (FileStream stream = File.OpenWrite(tempBarcodePath))
                {
                    data.SaveTo(stream);
                }

                // Path to the logo image
                string logoImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "ciusss.png");

                // PDF Document Setup
                using (PdfWriter writer = new PdfWriter(pdfPath))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf, new PageSize(216, 396)))
                {
                    int numberOfPages = pdf.GetNumberOfPages();

                    document.SetRenderer(new DocumentRenderer(document));
                    document.SetMargins(10, 10, 10, 10);

                    // Logo
                    if (File.Exists(logoImagePath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoImagePath);
                        Image logo = new Image(imageData);
                        logo.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                        logo.ScaleToFit(80, 80); // Make the logo smaller

                        // Get the page size from the document
                        PageSize pageSize = document.GetPdfDocument().GetDefaultPageSize();
                        float pageHeight = pageSize.GetHeight();

                        // Position in top left corner
                        logo.SetFixedPosition(10, pageHeight - 60);
                        document.Add(logo);
                    }

                    // Add some space after the logo
                    document.Add(new Paragraph("\n\n\n\n"));

                    // Title
                    Paragraph title = new Paragraph("Billet de stationnement")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(14);
                    document.Add(title);

                    // Add some space after the title
                    document.Add(new Paragraph("\n"));

                    // Ticket Details
                    Table table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    void AddTableCell(string label, string value)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(label).SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                        table.AddCell(new Cell().Add(new Paragraph(value).SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA))).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    }

                    AddTableCell("ID:", ticket.Id.ToString());
                    AddTableCell("Plaque:", ticket.LicensePlate);
                    AddTableCell("Arrivée:", ticket.ArrivalTime.ToString("dd/MM/yyyy HH:mm"));
                    document.Add(table);

                    // Add some space before the barcode
                    document.Add(new Paragraph("\n\n\n"));

                    // Barcode
                    ImageData barcodeImageData = ImageDataFactory.Create(tempBarcodePath);
                    Image barcodeImage = new Image(barcodeImageData);
                    barcodeImage.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    barcodeImage.ScaleToFit(200, 80);
                    document.Add(barcodeImage);
                }

                // Clean up temp file
                File.Delete(tempBarcodePath);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfPath)
                {
                    UseShellExecute = true
                });

                Console.WriteLine($"PDF ticket created successfully: {pdfPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
