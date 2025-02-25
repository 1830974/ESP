using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Administration.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts.Wpf;
using LiveCharts;
using Administration.Data.Context;
using Microsoft.Extensions.Configuration;
using Administration.Data;
using Microsoft.EntityFrameworkCore;
using Xceed.Wpf.AvalonDock.Properties;
using Administration.Resources;

namespace Administration.ViewModels
{
    public partial class DashboardVM : ObservableObject
    {
        private readonly CiusssContext _context;
        private readonly string _apiKey;

        public DashboardVM(CiusssContext context,  IConfiguration configuration) 
        {
            _context = context;
            _apiKey = configuration["ApiKey"];

            InitializeChartData();
        }

        #region VisualCharts
        [ObservableProperty]
        private SeriesCollection _pieSeriesCollection;

        [ObservableProperty]
        private SeriesCollection _cartesianSeriesCollection;

        [ObservableProperty]
        private IList<string> _pieLabels;

        [ObservableProperty]
        private IList<string> _cartesianLabels;

        [ObservableProperty]
        private Func<double, string> _formatter;

        [ObservableProperty]
        private string _selectedChartPointInfo;

        [ObservableProperty]
        private double _totalRevenue;

        [ObservableProperty]
        private List<PieChartDataItem> _pieChartDataItems;

        public async Task InitializeChartData()
        {
            Parking? parking = await ApiHelper.GetParkingState(_apiKey);
            List<WeeklyRevenue> weeklyRevenues = await GetWeeklyRevenues();
            PieLabels = new List<string> { Resources.Strings.UnpaidTickets, Resources.Strings.AvailableSpaces };

            PieSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = Resources.Strings.UnpaidTickets,
                    Values = new ChartValues<double> { parking.OccupiedSpaces },
                },
                new PieSeries
                {
                    Title = Resources.Strings.AvailableSpaces,
                    Values = new ChartValues<double> { parking.AllSpaces - parking.OccupiedSpaces },
                }
            };

            CartesianSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = Resources.Strings.Revenue,
                    Values = new ChartValues<double>(weeklyRevenues.Select(wr => (double)wr.TotalRevenue))
                }
            };

            CartesianLabels = Enumerable.Range(1, 7)
                .Select(i => DateTime.Now.AddDays(-i).ToString("dd/MM"))
                .Reverse()
                .ToList();

            Formatter = value => value.ToString("C2", new System.Globalization.CultureInfo("en-US"));

            UpdatePieChartDataItems();
            UpdateTotalRevenue();
        }

        [RelayCommand]
        private void PieChartClick(ChartPoint chartPoint)
        {
            if (chartPoint != null)
            {
                SelectedChartPointInfo = $"You clicked on {chartPoint.SeriesView.Title}: {chartPoint.Y}";
            }
        }

        private void UpdatePieChartDataItems()
        {
            PieChartDataItems = PieSeriesCollection.Select(series => new PieChartDataItem
            {
                Title = series.Title,
                Value = series.Values.Cast<double>().FirstOrDefault()
            }).ToList();
        }

        private void UpdateTotalRevenue()
        {
            TotalRevenue = CartesianSeriesCollection[0].Values.Cast<double>().Sum();
        }

        private async Task<List<WeeklyRevenue>> GetWeeklyRevenues()
        {
            DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = monday.AddDays(6);

            var weekDays = Enumerable.Range(0, 7)
                .Select(offset => monday.AddDays(offset))
                .ToDictionary(date => date.Date, date => new WeeklyRevenue
                {
                    Date = date,
                    TotalRevenue = 0,
                    TotalTPS = 0,
                    TotalTVQ = 0,
                    ReceiptCount = 0
                });

            var receiptsData = await _context.Reciepts
                .Include(r => r.Ticket)
                .Where(r => r.Ticket.PaymentTime >= monday && r.Ticket.PaymentTime <= endOfWeek)
                .GroupBy(r => r.Ticket.PaymentTime.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    TotalRevenue = group.Sum(r => r.Total),
                    TotalTPS = group.Sum(r => r.TPS),
                    TotalTVQ = group.Sum(r => r.TVQ),
                    ReceiptCount = group.Count()
                })
                .ToListAsync();

            foreach (var receipt in receiptsData)
            {
                if (weekDays.TryGetValue(receipt.Date, out var weekRevenue))
                {
                    weekRevenue.TotalRevenue = receipt.TotalRevenue;
                    weekRevenue.TotalTPS = receipt.TotalTPS;
                    weekRevenue.TotalTVQ = receipt.TotalTVQ;
                    weekRevenue.ReceiptCount = receipt.ReceiptCount;
                }
            }

            return weekDays.Values.OrderBy(wr => wr.Date).ToList();
        }
        #endregion

        private class WeeklyRevenue
        {
            public DateTime Date { get; set; }
            public double TotalRevenue { get; set; }
            public double TotalTPS { get; set; }
            public double TotalTVQ { get; set; }
            public int ReceiptCount { get; set; }
        }
    }
}
