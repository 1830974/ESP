using Administration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
