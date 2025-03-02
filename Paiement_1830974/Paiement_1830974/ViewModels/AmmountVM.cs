using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Resources;
using Paiement_1830974.Models;
using Paiement_1830974.Data;
using CommunityToolkit.Mvvm.Input;

namespace Paiement_1830974.ViewModels
{
    public partial class AmmountVM : ObservableObject
    {
        private readonly string _apiKey;
        private readonly INavigationService _navigationService;
        private readonly Ticket _ticket;
        [ObservableProperty] private string _ammount;

        public AmmountVM(IConfiguration configuration, INavigationService navigationService)
        {
            _apiKey = configuration["ApiKey"];
            _navigationService = navigationService;
            _ticket = TicketHolder.CurrentTicket;
        }

        public static async Task<AmmountVM> CreateAsync(IConfiguration configuration, INavigationService navigationService)
        {
            var viewModel = new AmmountVM(configuration, navigationService);
            await viewModel.InitializeAsync();
            return viewModel;
        }

        private async Task InitializeAsync()
        {
            double calculatedPrice = await CalculateTicketPrice();
            Ammount = calculatedPrice.ToString("C2");
        }

        [RelayCommand]
        private void DebitCreditRedirection()
            => _navigationService.NavigateTo<NIPVM>();

        private async Task<double> CalculateTicketPrice()
        {
            IEnumerable<Prices> activePrices = await ApiHelper.GetPricesForTicket(_ticket, _apiKey);

            if (activePrices == null || !activePrices.Any())
            {
                throw new InvalidOperationException("No active prices found for the ticket.");
            }

            var dailyPrice = activePrices.FirstOrDefault(p => p.PriceName == "Journée complète")?.Price ?? double.MaxValue;
            var halfDayPrice = activePrices.FirstOrDefault(p => p.PriceName == "Demi-journée")?.Price ?? double.MaxValue;
            var hourlyPrice = activePrices.FirstOrDefault(p => p.PriceName == "Horraire")?.Price ?? double.MaxValue;

            double totalPrice = 0;
            TimeSpan duration = DateTime.Now - _ticket.ArrivalTime;

            int fullDays = (int)duration.TotalDays;
            totalPrice += fullDays * dailyPrice;
            duration -= TimeSpan.FromDays(fullDays);

            if (duration.TotalHours > 12 || (duration.TotalHours > 0 && halfDayPrice < hourlyPrice * duration.TotalHours))
            {
                totalPrice += halfDayPrice;
                duration -= TimeSpan.FromHours(12);
            }

            if (duration > TimeSpan.Zero)
                totalPrice += Math.Ceiling(duration.TotalHours) * hourlyPrice;

            return totalPrice;
        }
    }
}
