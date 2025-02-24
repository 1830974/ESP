using Administration.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Administration.ViewModels
{
    public partial class HomeVM : ObservableObject
    {
        public HomeVM()
        {
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

        private void InitializeChartData()
        {
            PieSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Tickets actifs non payés",
                    Values = new ChartValues<double> { 153 },
                },
                new PieSeries
                {
                    Title = "Places disponibles",
                    Values = new ChartValues<double> { 42 },
                }
            };

            UpdatePieChartDataItems();

            PieLabels = new List<string> { "Tickets non payés", "Places disponibles" };

            CartesianSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Revenus",
                    Values = new ChartValues<double> { 400.24, 629.14, 564.78, 295.41, 726.89, 502.01, 666.66 }
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
        #endregion
    }
}
