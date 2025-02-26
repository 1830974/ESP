using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Sortie_1830974.Data;
using Sortie_1830974.Models;

namespace Sortie_1830974.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        private readonly string _apiKey;
        private bool _isProcessing = false;
        private string _defaultMessage = "Veuillez passer votre billet sous le scanneur";
        private string _gateOpenMessage = "Ouverture de la barrière";

        [ObservableProperty]
        private string _displayMessage;

        public MainWindowVM()
        {
            
        }

        public MainWindowVM(IConfiguration configuration) : this()
        {
            _apiKey = configuration["ApiKey"];
            DisplayMessage = _defaultMessage;
        }

        [RelayCommand]
        private async Task VerifyScannedTicket(int ticketId)
        {
            if (_isProcessing) return;

            _isProcessing = true;
            DisplayMessage = _gateOpenMessage;

            try
            {
                var (ticket, errorMessage) = await ApiHelper.GetTicketById(ticketId, _apiKey);

                if (ticket == null || ticket.State != "Payé")
                {
                    DisplayMessage = errorMessage ?? "An unknown error occurred";
                    await Task.Delay(10000);
                    return;
                }

                DisplayMessage = _gateOpenMessage;
                await Task.Delay(20000);
            }
            catch (Exception ex)
            {
                DisplayMessage = $"An error occurred: {ex.Message}";
                await Task.Delay(5000);
            }
            finally
            {
                DisplayMessage = _defaultMessage;
                _isProcessing = false;
            }
        }
    }
}
