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
using Microsoft.Extensions.DependencyInjection;
using ParkingApp_1830974.ViewModels;

namespace ParkingApp_1830974.Views
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private readonly LicensePlateVM _licensePlateVM;

        public Home(LicensePlateVM licensePlateVM)
        {
            InitializeComponent();
            this.DataContext = licensePlateVM;

            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var licenseList = serviceProvider.GetRequiredService<LicenseList>();
            MainFrame.Navigate(licenseList);
        } 

        private void NavigateToLicensePlates(object sender, NavigationEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var licenseList = serviceProvider.GetRequiredService<LicenseList>();
            MainFrame.Navigate(licenseList);
        }
    }
}
