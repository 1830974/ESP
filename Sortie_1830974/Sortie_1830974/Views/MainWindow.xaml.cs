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
using System.Windows.Threading;
using Sortie_1830974.ViewModels;

namespace Sortie_1830974.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string barcodeBuffer = string.Empty;
        private DispatcherTimer barcodeTimer;

        public MainWindow(MainWindowVM mainWindowVM)
        {
            InitializeComponent();
            this.DataContext = mainWindowVM;

            barcodeTimer = new DispatcherTimer();
            barcodeTimer.Interval = TimeSpan.FromMilliseconds(50);
            barcodeTimer.Tick += BarcodeTimer_Tick;
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
                var viewModel = DataContext as MainWindowVM;
                viewModel?.VerifyScannedTicketCommand.Execute(barcodeBuffer);
                barcodeBuffer = string.Empty;
            }
        }
    }
}