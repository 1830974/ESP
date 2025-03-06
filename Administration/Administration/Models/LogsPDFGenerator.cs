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
    public static class LogsPDFGenerator
    {
        public static string GenerateLogPDF(IEnumerable<Logs> logs, string logType, DateTime startDate, DateTime endDate)
        {
            string pdfPath = $"LogReport_{logType}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf";

            using (PdfWriter writer = new PdfWriter(pdfPath))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document document = new Document(pdf, PageSize.A4))
            {
                document.SetMargins(20, 20, 20, 20);

                // Add title
                document.Add(new Paragraph($"Log Report - {logType}")
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER));

                // Add date range
                document.Add(new Paragraph($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                // Create table
                Table table = new Table(new float[] { 1, 2, 2, 4 }).UseAllAvailableWidth();
                table.AddHeaderCell("ID");
                table.AddHeaderCell("Entry Time");
                table.AddHeaderCell("Origin");
                table.AddHeaderCell("Description");

                // Add log entries to table
                foreach (var log in logs)
                {
                    table.AddCell(log.Id.ToString());
                    table.AddCell(log.EntryTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    table.AddCell(log.Origin);
                    table.AddCell(log.Description);
                }

                document.Add(table);
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfPath)
            {
                UseShellExecute = true
            });

            return pdfPath;
        }
    }
}
