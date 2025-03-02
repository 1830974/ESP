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
using Paiement_1830974.Resources;
using Paiement_1830974.ViewModels;

namespace Paiement_1830974.Views
{
    /// <summary>
    /// Logique d'interaction pour NIP.xaml
    /// </summary>
    public partial class NIP : Page
    {
        public NIP(NIPVM nipVM)
        {
            InitializeComponent();
            this.DataContext = nipVM;
        }

        private void PinPasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                PasswordHelper.AttachBoundNip(passwordBox);
            }
        }
    }
}
