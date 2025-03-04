using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Resources;

namespace Paiement_1830974.ViewModels
{
    public partial class NIPVM : ObservableObject
    {
        private readonly string _apiKey;
        private readonly INavigationService _navigationService;
        [ObservableProperty] private int _nip;
        [ObservableProperty] private string _errorMessage;
        [ObservableProperty] private bool _showError;

        public NIPVM(IConfiguration configuration, INavigationService navigationService)
        {
            _apiKey = configuration["ApiKey"];
            _navigationService = navigationService;
            ErrorMessage = "NIP Invalide";
        }

        [RelayCommand]
        private void Confirm()
        {
            if (IsAccepted())
            {
                ShowError = false;
                Debug.WriteLine($"Do Something");
                _navigationService.NavigateTo<BankConfirmVM>();
            }
            else
            {
                ShowError = true;
                ErrorMessage = "NIP Invalide";
                Debug.WriteLine("Do something else");
            }
        }

        private bool CanConfirm()
            => Nip >= 1000 && Nip <= 9999;

        private bool IsAccepted()
            => CanConfirm() && Nip == 9999;
    }
}
