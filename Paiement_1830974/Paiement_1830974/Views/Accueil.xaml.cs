using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Paiement_1830974.ViewModels;
using System.Windows.Controls;

namespace Paiement_1830974.Views
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Page
    {
        private string barcodeBuffer = string.Empty;
        private DispatcherTimer barcodeTimer;

        public Accueil(AccueilVM acceuilVM)
        {
            InitializeComponent();
            this.DataContext = acceuilVM;

            barcodeTimer = new DispatcherTimer();
            barcodeTimer.Interval = TimeSpan.FromMilliseconds(50);
            barcodeTimer.Tick += BarcodeTimer_Tick;

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.PreviewKeyDown += Window_PreviewKeyDown;
            }

            this.Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessBarcode();
            }
            else
            {
                barcodeBuffer += e.Key.ToString();
                barcodeTimer.Stop();
                barcodeTimer.Start();
            }
            e.Handled = true;
        }

        private void BarcodeTimer_Tick(object sender, EventArgs e)
        {
            barcodeTimer.Stop();
            ProcessBarcode();
        }

        private void ProcessBarcode()
        {
            if (!string.IsNullOrEmpty(barcodeBuffer))
            {
                var viewModel = DataContext as AccueilVM;
                string trimmedBuffer = barcodeBuffer.Replace("D", "");
                int.TryParse(trimmedBuffer, out int ticketId);
                viewModel?.FetchScannedTicketCommand.Execute(ticketId);
                barcodeBuffer = string.Empty;
            }
        }

        private void Accueil_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.PreviewKeyDown -= Window_PreviewKeyDown;
            }
        }
    }
}
