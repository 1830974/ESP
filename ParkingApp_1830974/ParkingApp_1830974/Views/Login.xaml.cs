using Microsoft.Extensions.DependencyInjection;
using ParkingApp_1830974.ViewModels;
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

namespace ParkingApp_1830974
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login(LoginVM loginVM)
        {
            InitializeComponent();
            this.DataContext = loginVM;
        }
    }
}