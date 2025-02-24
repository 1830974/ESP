using Administration.ViewModels;
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

namespace Administration.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            this.DataContext = new HomeVM();
            MainFrame.Navigate(new DashboardPage());
        }
        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage());
        }

        private void NavigateToManagement(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ManagementPage());
        }

        private void NavigateToReport(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportPage());
        }
    }
}