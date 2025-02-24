using ParkingApp_1830974.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.Extensions.Configuration;
using ParkingApp_1830974.Data.Context;
using Entree_1830974.Data;

namespace ParkingApp_1830974.ViewModels
{
    public partial class LicensePlateVM : ObservableObject
    {
        private readonly CiusssContext _context;
        private readonly string _apiKey;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<LicensePlateDTO> _licensePlates;

        private ICollectionView _filteredLicensePlates;
        public ICollectionView FilteredLicensePlates
        {
            get => _filteredLicensePlates;
            set => SetProperty(ref _filteredLicensePlates, value);
        }

        public LicensePlateVM(CiusssContext context, IConfiguration configuration)
        {
            _context = context;
            _apiKey = configuration["ApiKey"];

            LicensePlates = new ObservableCollection<LicensePlateDTO>();
            LoadLicensePlatesAsync();

            FilteredLicensePlates = CollectionViewSource.GetDefaultView(LicensePlates);
            FilteredLicensePlates.Filter = FilterLicensePlates;
        }

        private async Task LoadLicensePlatesAsync()
        {
            try
            {
                var plates = await ApiHelper.GetAllActiveLicensePlates(_apiKey);
                LicensePlates = new ObservableCollection<LicensePlateDTO>(plates);
                OnPropertyChanged(nameof(LicensePlates));
                FilteredLicensePlates = CollectionViewSource.GetDefaultView(LicensePlates);
                FilteredLicensePlates.Filter = FilterLicensePlates;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading license plates: {ex.Message}");
            }
        }

        private bool FilterLicensePlates(object obj)
        {
            if (string.IsNullOrEmpty(SearchText))
                return true;

            if (obj is LicensePlateDTO licensePlate)
            {
                return licensePlate.LicensePlate.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }

        partial void OnSearchTextChanged(string value)
        {
            FilteredLicensePlates?.Refresh();
        }
    }
}
