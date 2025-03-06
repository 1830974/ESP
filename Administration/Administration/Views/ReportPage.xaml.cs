using Administration.Models.Interfaces;
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
using Administration.ViewModels;

namespace Administration.Views
{
    /// <summary>
    /// Logique d'interaction pour ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page, ILanguageRefresher
    {
        public ReportPage(ReportVM reportVM)
        {
            InitializeComponent();
            this.DataContext = reportVM;
        }

        public async Task RefreshLanguage()
        {
            
        }
    }
}
