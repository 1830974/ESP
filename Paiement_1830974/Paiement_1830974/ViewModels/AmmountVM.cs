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
            var activePrices = await GetActivePrices();
            var priceRates = ExtractPriceRates(activePrices);
            var duration = DateTime.Now - _ticket.ArrivalTime;

            var (totalPrice, revenueType) = CalculateTotalPrice(duration, priceRates);

            UpdateRevenueTypeHolder(totalPrice, revenueType);

            return totalPrice;
        }

        private async Task<IEnumerable<Prices>> GetActivePrices()
        {
            var activePrices = await ApiHelper.GetPricesForTicket(_ticket, _apiKey);
            if (activePrices == null || !activePrices.Any())
            {
                throw new InvalidOperationException("No active prices found for the ticket.");
            }
            return activePrices;
        }

        private (double Daily, double HalfDay, double Hourly) ExtractPriceRates(IEnumerable<Prices> activePrices)
        {
            return (
                Daily: activePrices.FirstOrDefault(p => p.PriceName == "Journée complète")?.Price ?? double.MaxValue,
                HalfDay: activePrices.FirstOrDefault(p => p.PriceName == "Demi-journée")?.Price ?? double.MaxValue,
                Hourly: activePrices.FirstOrDefault(p => p.PriceName == "Horraire")?.Price ?? double.MaxValue
            );
        }

        private (double TotalPrice, string RevenueType) CalculateTotalPrice(TimeSpan duration, (double Daily, double HalfDay, double Hourly) rates)
        {
            double totalPrice = 0;
            string revenueType = "";
            bool halfDayApplied = false;

            int fullDays = (int)duration.TotalDays;
            totalPrice += fullDays * rates.Daily;
            duration -= TimeSpan.FromDays(fullDays);

            if (fullDays > 1)
                revenueType = "FullDay";

            if (ShouldApplyHalfDay(duration, rates.HalfDay, rates.Hourly))
            {
                totalPrice += rates.HalfDay;
                duration -= TimeSpan.FromHours(12);
                if (fullDays == 0) revenueType = "HalfDay";
                halfDayApplied = true;
            }

            if (duration > TimeSpan.Zero)
            {
                totalPrice += Math.Ceiling(duration.TotalHours) * rates.Hourly;
                if (!halfDayApplied) revenueType = "Hourly";
            }

            return (totalPrice, revenueType);
        }

        private bool ShouldApplyHalfDay(TimeSpan duration, double halfDayRate, double hourlyRate)
        {
            return duration.TotalHours > 12 || (duration.TotalHours > 0 && halfDayRate < hourlyRate * duration.TotalHours);
        }

        private void UpdateRevenueTypeHolder(double totalPrice, string revenueType)
        {
            RevenueTypeHolder.Monday = RevenueTypeHolder.getLastMonday();
            RevenueTypeHolder.Amount = totalPrice;
            RevenueTypeHolder.RevenueType = revenueType;
        }
    }
}
