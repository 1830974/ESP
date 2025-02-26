using Administration.ViewModels;
using System.Windows.Controls;
using Administration.Models.Interfaces;

namespace Administration.Views
{
    /// <summary>
    /// Logique d'interaction pour DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page, ILanguageRefresher
    {
        private readonly DashboardVM _viewModel;

        public DashboardPage(DashboardVM dashboardVM)
        {
            InitializeComponent();
            _viewModel = dashboardVM;
            this.DataContext = dashboardVM;
        }

        public async Task RefreshLanguage()
        {
            await _viewModel.InitializeChartData();
        }
    }
}
