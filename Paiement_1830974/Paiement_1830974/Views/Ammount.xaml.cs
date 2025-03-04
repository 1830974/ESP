using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Data.Context;
using Paiement_1830974.Resources;
using Paiement_1830974.ViewModels;

namespace Paiement_1830974.Views
{
    public partial class Ammount : Page
    {
        private readonly IConfiguration _configuration;
        private readonly INavigationService _navigationService;
        private readonly CiusssContext _context;

        public Ammount(CiusssContext context, IConfiguration configuration, INavigationService navigationService)
        {
            InitializeComponent();
            _context = context;
            _configuration = configuration;
            _navigationService = navigationService;
            Loaded += Ammount_Loaded;
        }

        private async void Ammount_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingIndicator.Visibility = Visibility.Visible;

                var viewModel = await AmmountVM.CreateAsync(_context, _configuration, _navigationService);
                DataContext = viewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingIndicator.Visibility = Visibility.Collapsed;
            }
        }
    }
}
