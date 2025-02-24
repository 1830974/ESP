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

        private async Task InitializeChartData()
        {
            Parking? parking = await ApiHelper.GetParkingState(_apiKey);

            PieSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Tickets actifs non payés",
                    Values = new ChartValues<double> { parking.OccupiedSpaces },
                },
                new PieSeries
                {
                    Title = "Places disponibles",
                    Values = new ChartValues<double> { parking.AllSpaces - parking.OccupiedSpaces },
                }
            };

            UpdatePieChartDataItems();

            PieLabels = new List<string> { "Tickets non payés", "Places disponibles" };

            List<object> wR = await GetWeeklyRevenues();

            CartesianSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Revenus",
                    Values = new ChartValues<double> {  }
                }
            };

            CartesianLabels = Enumerable.Range(1, 7)
                .Select(i => DateTime.Now.AddDays(-i).ToString("dd/MM"))
                .Reverse()
                .ToList();

            Formatter = value => value.ToString("C2");

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

        private async Task<List<object>> GetWeeklyRevenues()
        {
            DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = monday.AddDays(6);

            var test = await _context.Reciepts
                .Include(r => r.Ticket)
                .GroupBy(r => r.Ticket.PaymentTime.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    TotalRevenue = group.Sum(r => r.Total),
                    TotalTPS = group.Sum(r => r.TPS),
                    TotalTVQ = group.Sum(r => r.TVQ),
                    ReceiptCount = group.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            /*var receipts = await _context.Reciepts
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
                .OrderBy(x => x.Date)
                .ToListAsync();*/

            Console.WriteLine("");
            return /*receipts.Cast*/new List<object>(test).ToList();
        }
        #endregion
    }
}
