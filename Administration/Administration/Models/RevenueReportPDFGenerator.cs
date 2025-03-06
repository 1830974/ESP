using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Administration.ViewModels.ReportVM;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using iText.Layout.Renderer;
using iText.Layout.Borders;
using iText.Kernel.Colors;

namespace Administration.Models
{
    public static class RevenueReportPDFGenerator
    {
        public static void GeneratePDF(RevenueReport report)
        {
            string pdfPath = $"RevenueReport_{report.StartDate:yyyyMMdd}_{report.EndDate:yyyyMMdd}.pdf";

            try
            {
                // Path to the logo image
                string logoImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "ciusss.png");

                // PDF Document Setup
                using (PdfWriter writer = new PdfWriter(pdfPath))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf, PageSize.A4))
                {
                    document.SetMargins(20, 20, 20, 20);

                    // Define Colors
                    Color headerColor = new DeviceRgb(79, 129, 189);
                    Color sectionHeaderColor = new DeviceRgb(79, 98, 189);
                    Color borderColor = new DeviceRgb(192, 192, 192);

                    // Define Borders
                    Border sectionBorder = new SolidBorder(borderColor, 1);

                    // Logo
                    if (File.Exists(logoImagePath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoImagePath);
                        Image logo = new Image(imageData);
                        logo.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                        logo.ScaleToFit(120, 120);
                        document.Add(logo);
                    }

                    // Report Header
                    Paragraph header = new Paragraph("Rapport des Revenus")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(18)
                        .SetFontColor(headerColor)
                        .SetMarginBottom(10);
                    document.Add(header);

                    // Date Range
                    Paragraph dateRange = new Paragraph($"Période: {report.StartDate:yyyy-MM-dd} au {report.EndDate:yyyy-MM-dd}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA))
                        .SetFontSize(12)
                        .SetMarginBottom(20);
                    document.Add(dateRange);

                    // Report Content Table
                    Table table = new Table(new float[] { 2, 1, 2 }); // 3 Columns: Description, Count, Revenue
                    table.SetWidth(UnitValue.CreatePercentValue(100));
                    table.SetBorder(sectionBorder); // Apply Border to the Table
                    table.SetMarginBottom(20); // Add Margin Below Table

                    // Table Headers
                    Cell headerCellType = new Cell().Add(new Paragraph("Type de Tarification").SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                    Cell headerCellNbTicket = new Cell().Add(new Paragraph("Nombre de Tickets").SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));
                    Cell headerCellRevenu = new Cell().Add(new Paragraph("Revenu").SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)));

                    headerCellType.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                    headerCellNbTicket.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                    headerCellRevenu.SetBackgroundColor(ColorConstants.LIGHT_GRAY);

                    table.AddHeaderCell(headerCellType);
                    table.AddHeaderCell(headerCellNbTicket);
                    table.AddHeaderCell(headerCellRevenu);

                    // Hourly
                    table.AddCell(new Paragraph("Horaire"));
                    table.AddCell(report.HourlyTicketsCount.ToString());
                    table.AddCell(new Paragraph($"{report.HourlyRevenue:N2} $"));

                    // Half-Day
                    table.AddCell(new Paragraph("Demi-Journée"));
                    table.AddCell(report.HalfDayTicketsCount.ToString());
                    table.AddCell(new Paragraph($"{report.HalfDayRevenue:N2} $"));

                    // Full-Day
                    table.AddCell(new Paragraph("Journée Complète"));
                    table.AddCell(report.FullDayTicketsCount.ToString());
                    table.AddCell(new Paragraph($"{report.FullDayRevenue:N2} $"));

                    // Total Revenue Before Taxes
                    table.AddCell(new Cell().Add(new Paragraph("Total (avant taxes)").SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))));
                    table.AddCell("");
                    table.AddCell(new Cell().Add(new Paragraph($"{report.HourlyRevenue + report.HalfDayRevenue + report.FullDayRevenue:N2} $").SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))));

                    // TPS
                    table.AddCell(new Paragraph("TPS"));
                    table.AddCell("");
                    table.AddCell(new Paragraph($"{report.TotalTPS:N2} $"));

                    // TVQ
                    table.AddCell(new Paragraph("TVQ"));
                    table.AddCell("");
                    table.AddCell(new Paragraph($"{report.TotalTVQ:N2} $"));

                    // Total Revenue (Including Taxes)
                    table.AddCell(new Cell().Add(new Paragraph("Total (taxes incluses)").SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))));
                    table.AddCell("");
                    table.AddCell(new Cell().Add(new Paragraph($"{report.TotalRevenue:N2} $").SetFont(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))));

                    document.Add(table);
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfPath)
                {
                    UseShellExecute = true
                });

                System.Console.WriteLine($"PDF generated successfully: {pdfPath}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error generating PDF: {ex.Message}");
            }
        }
    }
}
