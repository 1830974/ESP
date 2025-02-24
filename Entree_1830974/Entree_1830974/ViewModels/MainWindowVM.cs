using Entree_1830974.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entree_1830974.Models;
using Entree_1830974.Data;
using Entree_1830974.Ressources;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Entree_1830974.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        private readonly CiusssContext _context;
        private readonly string _apiKey;
        private bool _isProcessing = false;
        private string _defaultMessage = "Bienvenue au parking\n\nVeuillez appuyer sur le bouton pour obtenir\nvotre ticket";
        private string _gateOpenMessage = "Ouverture de la barrière";

        [ObservableProperty]
        private string _displayMessage;

        public MainWindowVM(CiusssContext context, IConfiguration configuration)
        {
            _context = context;
            _apiKey = configuration["ApiKey"];
            DisplayMessage = _defaultMessage;
        }

        [RelayCommand]
        private async Task EnterKeyPressed()
        {
            if (_isProcessing) return;

            _isProcessing = true;
            DisplayMessage = _gateOpenMessage;

            try
            {
                var (success, parking, errorMessage) = await ApiHelper.AddOccupiedSpace(_apiKey);

                if (!success || parking == null)
                {
                    DisplayMessage = errorMessage ?? "An unknown error occurred";
                    return;
                }   

                string licensePlate = LicensePlateGenerator.Generate();

                Ticket newTicket = await ApiHelper.GenerateTicket(licensePlate, _apiKey);

                string pdfPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ticket{newTicket.Id}.pdf");
                BarcodePdfGenerator.GenerateTicketPdf(newTicket, pdfPath);

                DisplayMessage = _gateOpenMessage;
                Debug.WriteLine($"Ticket generated. Spaces left: {parking.AllSpaces - parking.OccupiedSpaces}");
                 
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
