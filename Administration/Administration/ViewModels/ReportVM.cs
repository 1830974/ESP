using Administration.Data.Context;
using Administration.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.ViewModels
{
    public partial class ReportVM : ObservableObject
    {
        [ObservableProperty] private DateTime? revenueReportStartDate;
        [ObservableProperty] private DateTime? revenueReportEndDate;
        [ObservableProperty] private DateTime? debugReportStartDate;
        [ObservableProperty] private DateTime? debugReportEndDate;
        [ObservableProperty] private DateTime? logReportStartDate;
        [ObservableProperty] private DateTime? logReportEndDate;
        [ObservableProperty] private string logEntryType;
        //[ObservableProperty] private ObservableCollection<BlockedRequest> blockedRequests;

        private readonly CiusssContext _context;

        public ReportVM(CiusssContext context)
        {
            _context = context;
        }

        [RelayCommand]
        private async Task<string?> GenerateRevenueReport()
        {
            if (!RevenueReportStartDate.HasValue || !RevenueReportEndDate.HasValue)
                return "No date selected";

            DateTime startDate = RevenueReportStartDate.Value.Date;
            DateTime endDate = RevenueReportEndDate.Value.Date.AddDays(1).AddSeconds(-1); // Inclut la date de fin jusqu'à 11h59

            List<Reciept> receiptsInDateRange = await _context.Reciepts
                .Include(r => r.Ticket)
                .Where(r => r.Ticket.PaymentTime >= startDate && r.Ticket.PaymentTime <= endDate)
                .ToListAsync();

            if (!receiptsInDateRange.Any())
                return "No reciept found in the specified dates";

            List<Reciept> hourlyReceipts = new List<Reciept>();
            List<Reciept> halfDayReceipts = new List<Reciept>();
            List<Reciept> fullDayReceipts = new List<Reciept>();

            foreach (Reciept receipt in receiptsInDateRange)
            {
                TimeSpan duration = receipt.StayTime.Subtract(DateTime.MinValue); // retirer 0001-01-01

                if (duration <= TimeSpan.FromHours(1))
                    hourlyReceipts.Add(receipt);
                else if (duration <= TimeSpan.FromHours(4))
                    halfDayReceipts.Add(receipt);
                else
                    fullDayReceipts.Add(receipt);
            }

            double hourlyRevenue = hourlyReceipts.Sum(r => r.Total);
            double halfDayRevenue = halfDayReceipts.Sum(r => r.Total);
            double fullDayRevenue = fullDayReceipts.Sum(r => r.Total);

            double totalRevenue = hourlyRevenue + halfDayRevenue + fullDayRevenue;
            double totalTps = receiptsInDateRange.Sum(r => r.TPS);
            double totalTvq = receiptsInDateRange.Sum(r => r.TVQ);

            RevenueReport report = new RevenueReport()
            {
                StartDate = startDate,
                EndDate = endDate,
                HourlyTicketsCount = hourlyReceipts.Count,
                HalfDayTicketsCount = halfDayReceipts.Count,
                FullDayTicketsCount = fullDayReceipts.Count,
                HourlyRevenue = hourlyRevenue,
                HalfDayRevenue = halfDayRevenue,
                FullDayRevenue = fullDayRevenue,
                TotalRevenue = totalRevenue + totalTps + totalTvq,
                TotalTPS = totalTps,
                TotalTVQ = totalTvq
            };

            RevenueReportPDFGenerator.GeneratePDF(report);
            return "Report successfully generated";
        }

        public class RevenueReport
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int HourlyTicketsCount { get; set; }
            public int HalfDayTicketsCount { get; set; }
            public int FullDayTicketsCount { get; set; }
            public double HourlyRevenue { get; set; }
            public double HalfDayRevenue { get; set; }
            public double FullDayRevenue { get; set; }
            public double TotalRevenue { get; set; }
            public double TotalTPS { get; set; }
            public double TotalTVQ { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
