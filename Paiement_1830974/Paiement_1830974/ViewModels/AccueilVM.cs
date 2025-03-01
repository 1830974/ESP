using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Models;
using Paiement_1830974.Data;
using Paiement_1830974.Views;
using Paiement_1830974.Resources;


namespace Paiement_1830974.ViewModels
{
    public partial class AccueilVM : ObservableObject
    {
        private readonly string _apiKey;
        private readonly INavigationService _navigationService;
        private string defaultUpperMessage = "Veuillez scanner votre ticket";
        private string defaulLowerMessage = "Passez le code-barres de votre ticket sous le scanner";

        [ObservableProperty] private string _upperDisplayMessage;

        [ObservableProperty] private string _lowerDisplayMessage;

        public AccueilVM()
        {

        }

        public AccueilVM(IConfiguration configuration, INavigationService navigationService) : this()
        {
            _apiKey = configuration["ApiKey"];
            UpperDisplayMessage = defaultUpperMessage;
            LowerDisplayMessage = defaulLowerMessage;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task FetchScannedTicket(int ticketId)
        {
            try
            {
                var (ticket, errorMessage) = await ApiHelper.GetTicketById(ticketId, _apiKey);

                if (ticket == null || ticket.State == "Payé")
                {
                    UpperDisplayMessage = errorMessage ?? "An unknown error occurred";
                    await Task.Delay(10000);
                    return;
                }

                TicketHolder.CurrentTicket = ticket;
                _navigationService.NavigateTo<AmmountVM>();
            }
            catch (Exception ex)
            {
                UpperDisplayMessage = $"An error occurred: {ex.Message}";
                await Task.Delay(5000);
            }
            finally
            {
                UpperDisplayMessage = defaultUpperMessage;
                LowerDisplayMessage = defaulLowerMessage;
            }
        }
    }
}
