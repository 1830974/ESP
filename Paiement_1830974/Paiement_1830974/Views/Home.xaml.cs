using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Paiement_1830974.ViewModels;

namespace Paiement_1830974.Views
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private readonly IServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;

        public Home(AccueilVM accueilVM, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.DataContext = this;

            _serviceProvider = serviceProvider;
            var accueil = _serviceProvider.GetRequiredService<Accueil>();

            MainFrame.Navigate(accueil);
        }
    }
}
