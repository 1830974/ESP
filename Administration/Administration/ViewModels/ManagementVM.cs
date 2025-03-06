using Administration.Data.Context;
using Administration.Models;
using Administration.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Administration.Resources;

namespace Administration.ViewModels
{
    public partial class ManagementVM : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<User> users;
        [ObservableProperty] private ObservableCollection<Ticket> tickets;

        [ObservableProperty] private double hourlyRate;
        [ObservableProperty] private double halfDayRate;
        [ObservableProperty] private double fullDayRate;
        [ObservableProperty] private double provincialTaxRate;
        [ObservableProperty] private double federalTaxRate;

        private readonly CiusssContext _context;

        public ManagementVM(CiusssContext context)
        {
            _context = context;
            Users = new ObservableCollection<User>();
            Tickets = new ObservableCollection<Ticket>();
            LoadUsers();
            LoadRates();
            LoadTickets();
        }

        private void LoadUsers()
        {
            Users = new ObservableCollection<User>(_context.Users.ToList());
        }

        private void LoadTickets()
        {
            Tickets = new ObservableCollection<Ticket>(_context.Tickets);
        }

        private void LoadRates()
        {
            HourlyRate = _context.Prices
                .Where(p => p.PriceName == "Horraire")
                .OrderBy(p => p.Id)
                .Last().Price;

            HalfDayRate = _context.Prices
                .Where(p => p.PriceName == "Demi-Journée")
                .OrderBy(p => p.Id)
                .Last().Price;

            FullDayRate = _context.Prices
                .Where(p => p.PriceName == "Journée complète")
                .OrderBy(p => p.Id)
                .Last().Price;

            ProvincialTaxRate = _context.OtherValues.FirstOrDefault()?.TPS ?? 0;
            FederalTaxRate = _context.OtherValues.FirstOrDefault()?.TVQ ?? 0;
        }

        [RelayCommand]
        private async Task AddUser()
        {
            var createUserWindow = new CreateUser();
            createUserWindow.Owner = Application.Current.MainWindow;
            createUserWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (createUserWindow.ShowDialog() == true)
            {
                var viewModel = (CreateUserVM)createUserWindow.DataContext;
                User newUser = new User(viewModel.Username, PasswordHelper.HashPassword(viewModel.Password), viewModel.FirstName, viewModel.LastName, viewModel.Email, viewModel.State);

                Logs userCreateLog = new Logs()
                {
                    Id = 0,
                    EntryTime = DateTime.Now,
                    Origin = "Création d'utilisateur",
                    Description = $"Création de l'utilisateur \"{newUser.Username}\"",
                };

                _context.Users.Add(newUser);
                _context.Logs.Add(userCreateLog);
                await _context.SaveChangesAsync();
                Users.Add(newUser);
            }
        }


        [RelayCommand]
        private async Task EditUser(User user)
        {
            User? modifiedUser = await _context.Users.FindAsync(user.Id);

            if (modifiedUser == null)
            {
                Debug.WriteLine($"Error editing user with ID {user.Id}");
                return;
            }

            modifiedUser.Username = user.Username;
            modifiedUser.FirstName = user.FirstName;
            modifiedUser.LastName = user.LastName;
            modifiedUser.Email = user.Email;
            modifiedUser.State = user.State;

            // Visual update of the modified user
            int index = Users.IndexOf(Users.FirstOrDefault(u => u.Id == user.Id));
            if (index != -1)
            {
                Users[index] = modifiedUser;
            }

            Logs userModifiedLog = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Modification d'utilisateur",
                Description = $"Modification de l'utilisateur \"{modifiedUser.Username}\"",
            };

            _context.Logs.Add(userModifiedLog);
            await _context.SaveChangesAsync();
        }

        [RelayCommand]
        private async Task DeleteUser(User user)
        {
            if (user != null)
            {
                Logs userDeletedLog = new Logs()
                {
                    Id = 0,
                    EntryTime = DateTime.Now,
                    Origin = "Suppression d'utilisateur",
                    Description = $"Suppression de l'utilisateur \"{user.Username}\"",
                };

                _context.Logs.Add(userDeletedLog);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                Users.Remove(user);
            }
        }

        [RelayCommand]
        private async Task DeleteTicket(Ticket ticket)
        {
            if (ticket != null)
            {
                Logs ticketDeletedLog = new Logs()
                {
                    Id = 0,
                    EntryTime = DateTime.Now,
                    Origin = "Suppression de billet",
                    Description = $"Suppression du billet \"{ticket.Id}\"",
                };

                _context.Logs.Add(ticketDeletedLog);
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                Tickets.Remove(ticket);
            }
        }

        private async Task UpdateRate(string priceName, double price)
        {
            var newPrice = new Prices
            {
                SetPriceDate = DateTime.Now,
                PriceName = priceName,
                Price = price
            };

            _context.Prices.Add(newPrice);

            await CreateLogAndSave("Tarifs", $"Modification du tarif \"{priceName}\" {price}$");
        }

        private async Task UpdateTaxRate(bool isProvincial, double rate)
        {
            var otherValues = await _context.OtherValues.FirstOrDefaultAsync() ?? new OtherValues();

            if (isProvincial)
                otherValues.TVQ = rate;
            else
                otherValues.TPS = rate;

            if (otherValues.Id == 0)
                _context.OtherValues.Add(otherValues);

            string taxType = isProvincial ? "provinciale (TVQ)" : "fédérale (TPS)";
            await CreateLogAndSave("Tarifs", $"Modification du taux de taxe {taxType} {rate}%");
        }

        private async Task CreateLogAndSave(string origin, string description)
        {
            var log = new Logs
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = origin,
                Description = description
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            LoadRates();
        }

        [RelayCommand]
        private Task UpdateHourlyRate() => UpdateRate("Horraire", HourlyRate);

        [RelayCommand]
        private Task UpdateHalfDayRate() => UpdateRate("Demi-Journée", HalfDayRate);

        [RelayCommand]
        private Task UpdateFullDayRate() => UpdateRate("Journée complète", FullDayRate);

        [RelayCommand]
        private Task UpdateProvincialTaxRate() => UpdateTaxRate(true, ProvincialTaxRate);

        [RelayCommand]
        private Task UpdateFederalTaxRate() => UpdateTaxRate(false, FederalTaxRate);
    }
}
