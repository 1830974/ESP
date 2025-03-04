using Accessibility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iText.StyledXmlParser.Node;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Data;
using Paiement_1830974.Data.Context;
using Paiement_1830974.Models;
using Paiement_1830974.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paiement_1830974.ViewModels
{
    public partial class BankConfirmVM : ObservableObject
    {
        private readonly string _apiKey;
        private readonly CiusssContext _context;
        private readonly INavigationService _navigationService;
        [ObservableProperty] private string _loadingMessage;

        public BankConfirmVM(CiusssContext context, IConfiguration configuration, INavigationService navigationService)
        {
            _apiKey = configuration["ApiKey"];
            _context = context;
            _navigationService = navigationService;
            LoadingMessage = "";
        }

        [RelayCommand]
        private async Task RedirectToReciept()
        {
            await AnimateLoadingMessage(4);

            // assume the bank confirmed the transaction
            RevenueByType currentWeeksRevenues = await FindCurrentWeekInRevenues();
            RevenueByType? weekToUpdate = await _context.RevenueByTypes.FindAsync(currentWeeksRevenues.Id);

            if (weekToUpdate == null)
                return;

            // Stop if the Api can't pay the ticket
            string? result = await ApiHelper.PayTicket(TicketHolder.CurrentTicket.Id, _apiKey);
            if (result == null)
                return;

            //Generate receipt in DB
            await GenerateReceipt();

            switch (RevenueTypeHolder.RevenueType)
            {
                case "Hourly":
                    weekToUpdate.Hourly += RevenueTypeHolder.Amount;
                    break;
                case "HalfDay":
                    weekToUpdate.HalfDay += RevenueTypeHolder.Amount;
                    break;
                case "FullDay":
                    weekToUpdate.FullDay += RevenueTypeHolder.Amount;
                    break;
                default:
                    Debug.WriteLine("Error updating the revenues");
                    break;
            }

            await _context.SaveChangesAsync();

            _navigationService.NavigateTo<RecieptVM>();
        }

        private async Task AnimateLoadingMessage(int loops)
        {
            for (int i = 0; i < loops * 3; i++)
            {
                int dotCount = (i % 3) + 1;
                LoadingMessage = $"En attente de confirmation{new string('.', dotCount)}";
                await Task.Delay(400);
            }
        }

        private async Task<RevenueByType> FindCurrentWeekInRevenues()
        {
            RevenueByType? revenueByType = await _context.RevenueByTypes.Where(r => r.Week == RevenueTypeHolder.Monday).FirstOrDefaultAsync();

            // If the last monday doesn't appear in the DB, create it
            if (revenueByType == null)
            {
                RevenueByType newWeek = new RevenueByType()
                {
                    Id = 0,
                    Week = RevenueTypeHolder.Monday,
                    Hourly = 0,
                    HalfDay = 0,
                    FullDay = 0,
                };

                _context.RevenueByTypes.Add(newWeek);
                await _context.SaveChangesAsync();

                return newWeek;
            }
            else
            {
                return revenueByType;
            }
        }

        private async Task GenerateReceipt()
        {
            Reciept receipt = new Reciept()
            {
                Id = 0,
                TicketId = TicketHolder.CurrentTicket.Id,
                StayTime = TicketHolder.StayTime,
                Total = PaymentHolder.TotalAmount,
                TPS = PaymentHolder.TPS,
                TVQ = PaymentHolder.TVQ,
            };

            _context.Reciepts.Add(receipt);
            await _context.SaveChangesAsync();
        }
    }
}
