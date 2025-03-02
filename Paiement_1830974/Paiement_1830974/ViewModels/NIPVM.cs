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
        [ObservableProperty]
        private int _nip;

        public NIPVM(IConfiguration configuration, INavigationService navigationService)
        {
            _apiKey = configuration["ApiKey"];
            _navigationService = navigationService;
        }

        partial void OnNipChanged(int value)
        {
            Console.WriteLine($"NIP changed to: {value}");
            CommandManager.InvalidateRequerySuggested();
        }

        [RelayCommand]
        private void Confirm()
        {
            if (CanConfirm())
                Debug.WriteLine($"Confirmation button clicked with NIP: {Nip}");
            else
                Debug.WriteLine("Bad NIP Length");
        }

        private bool CanConfirm()
        {
            return Nip >= 1000 && Nip <= 9999;
        }
    }
}
