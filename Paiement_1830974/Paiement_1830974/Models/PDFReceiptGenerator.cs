using System.IO;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using Paiement_1830974.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paiement_1830974.Models
{
    public static class PDFReceiptGenerator
    {
        public static void GeneratePDFReceipt(string? pdfPath = "LatestReceipt.pdf")
        {
            try
            {
                // Path to the logo image
                string logoImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "ciusss.png");

                // PDF Document Setup
                using (PdfWriter writer = new PdfWriter(pdfPath))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf, PageSize.A5))
                {
                    document.SetRenderer(new DocumentRenderer(document));
                    document.SetMargins(20, 20, 20, 20);

                    // Logo
                    if (File.Exists(logoImagePath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoImagePath);
                        Image logo = new Image(imageData);
                        logo.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        logo.ScaleToFit(150, 150);
                        document.Add(logo);
                    }

                    // Hospital Name
                    Paragraph hospitalName = new Paragraph("Hôpital de Chicoutimi")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(16);
                    document.Add(hospitalName);

                    // Title
                    Paragraph title = new Paragraph("Reçu de transaction")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(14)
                        .SetMarginTop(20);
                    document.Add(title);

                    // Receipt Details
                    Table table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100));
                    table.SetMarginTop(20);

                    void AddTableCell(string label, string value)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(label).SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                        table.AddCell(new Cell().Add(new Paragraph(value).SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA))).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    }

                    AddTableCell("Heure d'arrivée:", TicketHolder.CurrentTicket.ArrivalTime.ToString("dd-MM-yyyy HH:mm"));
                    AddTableCell("Heure de sortie:", DateTime.Now.ToString("dd-MM-yyyy HH:mm"));

                    // Calculate and format stay duration
                    DateTime departureTime = DateTime.Now;
                    TimeSpan stayDuration = departureTime - TicketHolder.CurrentTicket.ArrivalTime;

                    int months = 0;
                    int days = stayDuration.Days;
                    int years = 0;

                    if (days >= 365)
                    {
                        years = days / 365;
                        days %= 365;
                    }

                    if (days >= 30)
                    {
                        months = days / 30;
                        days %= 30;
                    }

                    string formattedDuration = "";
                    if (years > 0)
                        formattedDuration += $"{years} an{(years > 1 ? "s" : "")}, ";
                    if (months > 0)
                        formattedDuration += $"{months} mois, ";
                    formattedDuration += $"{days} jour{(days > 1 ? "s" : "")}, ";
                    formattedDuration += $"{stayDuration.Hours:D2}:{stayDuration.Minutes:D2}:{stayDuration.Seconds:D2}";

                    AddTableCell("Montant de base:", $"{PaymentHolder.BaseAmount:N2} $");
                    AddTableCell("TPS:", $"{PaymentHolder.TPS:N2} $");
                    AddTableCell("TVQ:", $"{PaymentHolder.TVQ:N2} $");
                    AddTableCell("Montant total:", $"{PaymentHolder.TotalAmount:N2} $");

                    document.Add(table);

                    // Thank you message
                    Paragraph thankYou = new Paragraph("Merci de votre visite!")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_OBLIQUE))
                        .SetFontSize(12)
                        .SetMarginTop(20);
                    document.Add(thankYou);
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfPath)
                {
                    UseShellExecute = true
                });

                Console.WriteLine($"PDF receipt created successfully: {pdfPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
