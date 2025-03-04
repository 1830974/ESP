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
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using Paiement_1830974.Data.Context;

namespace Paiement_1830974.ViewModels
{
    public partial class AmmountVM : ObservableObject
    {
        private readonly string _apiKey;
        private CiusssContext _context;
        private readonly INavigationService _navigationService;
        private readonly Ticket _ticket;
        [ObservableProperty] private string _ammount;

        public AmmountVM(CiusssContext context, IConfiguration configuration, INavigationService navigationService)
        {
            _apiKey = configuration["ApiKey"];
            _context = context;
            _navigationService = navigationService;
            _ticket = TicketHolder.CurrentTicket;
        }

        public static async Task<AmmountVM> CreateAsync(CiusssContext context, IConfiguration configuration, INavigationService navigationService)
        {
            var viewModel = new AmmountVM(context, configuration, navigationService);
            viewModel._context = context;
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
            IEnumerable<Prices> activePrices = await GetActivePrices();
            var taxes = await GetTaxRates();
            var priceRates = ExtractPriceRates(activePrices);

            DateTime referenceDate = DateTime.MinValue;
            TimeSpan duration = DateTime.Now - _ticket.ArrivalTime;
            DateTime castedDuration = referenceDate.Add(duration);

            // Store StayTime for later useage
            TicketHolder.StayTime = castedDuration;

            var (totalPrice, revenueType) = CalculateTotalPrice(duration, priceRates, taxes);

            UpdateRevenueTypeHolder(totalPrice, revenueType);

            return totalPrice;
        }

        private async Task<IEnumerable<Prices>> GetActivePrices()
        {
            IEnumerable<Prices> activePrices = await ApiHelper.GetPricesForTicket(_ticket, _apiKey);
            if (activePrices == null || !activePrices.Any())
            {
                throw new InvalidOperationException("No active prices found for the ticket.");
            }
            return activePrices;
        }

        private async Task <(double TPS, double TVQ)> GetTaxRates()
        {
            // Taxes are always stored in the first index
            OtherValues? taxes = await _context.OtherValues.FindAsync(1);

            if (taxes == null)
                return (0, 0);

            return (taxes.TPS, taxes.TVQ);
        }

        private (double Daily, double HalfDay, double Hourly) ExtractPriceRates(IEnumerable<Prices> activePrices)
        {
            return (
                Daily: activePrices.FirstOrDefault(p => p.PriceName == "Journée complète")?.Price ?? double.MaxValue,
                HalfDay: activePrices.FirstOrDefault(p => p.PriceName == "Demi-journée")?.Price ?? double.MaxValue,
                Hourly: activePrices.FirstOrDefault(p => p.PriceName == "Horraire")?.Price ?? double.MaxValue
            );
        }

        private (double TotalPrice, string RevenueType) CalculateTotalPrice(TimeSpan duration, (double Daily, double HalfDay, double Hourly) rates, (double TPS, double TVQ) taxes)
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

            PaymentHolder.BaseAmount = totalPrice;

            double tpsAmount = Math.Round(totalPrice * taxes.TPS / 100, 2);
            double tvqAmount = Math.Round(totalPrice * taxes.TVQ / 100, 2);

            PaymentHolder.TPS = tpsAmount;
            PaymentHolder.TVQ = tvqAmount;

            totalPrice += tpsAmount;
            totalPrice += tvqAmount;

            PaymentHolder.TotalAmount = totalPrice;

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
