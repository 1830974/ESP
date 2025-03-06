using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.IO;
using static Administration.ViewModels.ReportVM;

namespace Administration.Models
{
    public static class DebugReportPDFGenerator
    {
        public static void GeneratePDF(DebugReport report)
        {
            string pdfPath = $"DebugReport_{report.StartDate:yyyyMMdd}_{report.EndDate:yyyyMMdd}.pdf";

            try
            {
                using (PdfWriter writer = new PdfWriter(pdfPath))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf, PageSize.A4.Rotate()))
                {
                    document.SetMargins(20, 20, 20, 20);

                    // Report Header
                    Paragraph header = new Paragraph("Debug Report")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18)
                        .SetMarginBottom(10);
                    document.Add(header);

                    // Date Range
                    Paragraph dateRange = new Paragraph($"Period: {report.StartDate:yyyy-MM-dd} to {report.EndDate:yyyy-MM-dd}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(12)
                        .SetMarginBottom(20);
                    document.Add(dateRange);

                    // Tickets Table
                    Table table = new Table(new float[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2 });
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    // Table Headers
                    string[] headers = { "Ticket ID", "Arrival Time", "Payment Time", "License Plate", "State", "Stay Time", "Total", "TPS", "TVQ", "Paid" };
                    foreach (string head in headers)
                    {
                        table.AddHeaderCell(new Cell().Add(new Paragraph(head)));
                    }

                    // Paid Tickets (with receipts)
                    foreach (var receipt in report.Receipts)
                    {
                        AddTicketRow(table, receipt.Ticket, receipt);
                    }

                    // Unpaid Tickets
                    foreach (var ticket in report.UnpaidTickets)
                    {
                        AddTicketRow(table, ticket, null);
                    }

                    document.Add(table);
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfPath)
                {
                    UseShellExecute = true
                });

                Console.WriteLine($"Debug PDF generated successfully: {pdfPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating Debug PDF: {ex.Message}");
            }
        }

        private static void AddTicketRow(Table table, Ticket ticket, Reciept receipt)
        {
            table.AddCell(ticket.Id.ToString());
            table.AddCell(ticket.ArrivalTime.ToString("yyyy-MM-dd HH:mm:ss"));
            table.AddCell(ticket.PaymentTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A");
            table.AddCell(ticket.LicensePlate);
            table.AddCell(ticket.State);

            if (receipt != null)
            {
                table.AddCell(receipt.StayTime.ToString(@"hh\:mm\:ss"));
                table.AddCell(receipt.Total.ToString("C2"));
                table.AddCell(receipt.TPS.ToString("C2"));
                table.AddCell(receipt.TVQ.ToString("C2"));
                table.AddCell("Yes");
            }
            else
            {
                table.AddCell("N/A");
                table.AddCell("N/A");
                table.AddCell("N/A");
                table.AddCell("N/A");
                table.AddCell("No");
            }
        }
    }
}
