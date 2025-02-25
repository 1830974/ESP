using Administration.Models.Interfaces;
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

namespace Administration.Views
{
    /// <summary>
    /// Logique d'interaction pour ManagementPage.xaml
    /// </summary>
    public partial class ManagementPage : Page, ILanguageRefresher
    {
        private int _selectedTabIndex;
        public ManagementPage()
        {
            InitializeComponent();
            this.DataContext = new ManagementVM();
        }

        public async Task RefreshLanguage()
        {

        }
    }
}
