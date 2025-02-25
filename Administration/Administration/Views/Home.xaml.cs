using Administration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Administration.Resources;
using System.Runtime.CompilerServices;
using Administration.Models.Interfaces;
using static MaterialDesignThemes.Wpf.Theme;

namespace Administration.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window, INotifyPropertyChanged
    {
        private readonly DashboardVM _dashBoardVM;
        private readonly IServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;

        public string DashboardText => Strings.Dashboard;
        public string HomeButtonText => Strings.HomeButton;
        public string ManagementText => Strings.Management;
        public string ReportText => Strings.Report;
        public string LanguageButtonText => Thread.CurrentThread.CurrentCulture.Name == "fr" ? "EN" : "FR";

        public Home(DashboardVM dashboardVM, IServiceProvider serviceProvider)
        {

            InitializeComponent();
            this.DataContext = this;

            _serviceProvider = serviceProvider;
            var dashboard = _serviceProvider.GetRequiredService<DashboardPage>();

            MainFrame.Navigate(dashboard);
        }
        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            var dashboard = _serviceProvider.GetRequiredService<DashboardPage>();
            MainFrame.Navigate(dashboard);
        }

        private void NavigateToManagement(object sender, RoutedEventArgs e)
        {
            var managementPage = _serviceProvider.GetRequiredService<ManagementPage>();
            MainFrame.Navigate(managementPage);
        }

        private void NavigateToReport(object sender, RoutedEventArgs e)
        {
            var reportPage = _serviceProvider.GetRequiredService<ReportPage>();
            MainFrame.Navigate(reportPage);
        }

        private void SwitchLanguage(object sender, RoutedEventArgs e)
        {
            string language = Thread.CurrentThread.CurrentCulture.Name == "fr" ? "en-US" : "fr";
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            OnPropertyChanged(nameof(DashboardText));
            OnPropertyChanged(nameof(HomeButtonText));
            OnPropertyChanged(nameof(ManagementText));
            OnPropertyChanged(nameof(ReportText));
            OnPropertyChanged(nameof(LanguageButtonText));

            var currentPageType = MainFrame.Content.GetType();

            var newPage = _serviceProvider.GetRequiredService(currentPageType);

            if (newPage is ILanguageRefresher refreshablePage)
            {
                refreshablePage.RefreshLanguage();
            }

            MainFrame.Navigate(newPage);
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}