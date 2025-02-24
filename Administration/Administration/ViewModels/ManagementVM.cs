using Administration.Models;
using Administration.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administration.ViewModels
{
    public partial class ManagementVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> users;

        [ObservableProperty]
        private ObservableCollection<Ticket> tickets;

        [ObservableProperty]
        private decimal hourlyRate;

        [ObservableProperty]
        private decimal halfDayRate;

        [ObservableProperty]
        private decimal fullDayRate;

        [ObservableProperty]
        private decimal provincialTaxRate;

        [ObservableProperty]
        private decimal federalTaxRate;

        public ManagementVM()
        {
            Users = new ObservableCollection<User>();
            Tickets = new ObservableCollection<Ticket>();
            LoadUsers();
            LoadRates();
            LoadTickets();
        }

        private void LoadUsers()
        {
            Users.Add(new User("jsmith", "pass123", "John", "Smith", "john.smith@email.com", true));
            Users.Add(new User("emiller", "securePass", "Emma", "Miller", "emma.miller@email.com", true));
            Users.Add(new User("dwilliams", "will1ams", "David", "Williams", "d.williams@email.com", false));
            Users.Add(new User("sbrown", "brownSue", "Susan", "Brown", "susan.brown@email.com", true));
            Users.Add(new User("mjohnson", "mikeJ2023", "Michael", "Johnson", "m.johnson@email.com", true));
            Users.Add(new User("ltaylor", "taylorL!", "Lisa", "Taylor", "lisa.taylor@email.com", false));
            Users.Add(new User("rclark", "clarkR1234", "Robert", "Clark", "robert.clark@email.com", true));
            Users.Add(new User("alee", "leeA2023", "Amanda", "Lee", "amanda.lee@email.com", true));
            Users.Add(new User("kwhite", "whiteK!", "Kevin", "White", "kevin.white@email.com", false));
            Users.Add(new User("ngreen", "greenN123", "Nancy", "Green", "nancy.green@email.com", true));
            Users.Add(new User("bhill", "hillB2023", "Brian", "Hill", "brian.hill@email.com", true));
            Users.Add(new User("cscott", "scottC!", "Carol", "Scott", "carol.scott@email.com", false));
            Users.Add(new User("jking", "kingJ1234", "James", "King", "james.king@email.com", true));
            Users.Add(new User("mross", "rossM2023", "Mary", "Ross", "mary.ross@email.com", true));
            Users.Add(new User("twood", "woodT!", "Thomas", "Wood", "thomas.wood@email.com", false));

            Users = new ObservableCollection<User>(Users.OrderBy(u => u.Username).OrderBy(u => u.State == false));
        }

        private void LoadTickets()
        {
            Tickets.Add(new Ticket(1, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), "ABC-123", "Active"));
            Tickets.Add(new Ticket(2, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-3), "DEF-456", "Active"));
            Tickets.Add(new Ticket(3, DateTime.Now.AddHours(-1), DateTime.Now, "GHI-789", "Paid"));
            Tickets.Add(new Ticket(4, DateTime.Now.AddDays(-1).AddHours(-4), DateTime.Now.AddDays(-1).AddHours(-2), "JKL-012", "Active"));
            Tickets.Add(new Ticket(5, DateTime.Now.AddHours(-3), DateTime.MinValue, "MNO-345", "paid"));
            Tickets.Add(new Ticket(6, DateTime.Now.AddDays(-2).AddHours(-6), DateTime.Now.AddDays(-2).AddHours(-5), "PQR-678", "Paid"));
            Tickets.Add(new Ticket(7, DateTime.Now.AddHours(-7), DateTime.Now.AddHours(-6), "STU-901", "Active"));
            Tickets.Add(new Ticket(8, DateTime.Now.AddDays(-3).AddHours(-10), DateTime.Now.AddDays(-3).AddHours(-8), "VWX-234", "Paid"));
            Tickets.Add(new Ticket(9, DateTime.Now.AddHours(-4), DateTime.MinValue, "YZ-567", "paid"));
            Tickets.Add(new Ticket(10, DateTime.Now.AddDays(-1).AddHours(-12), DateTime.Now.AddDays(-1).AddHours(-11), "BCD-890", "Paid"));

            Tickets = new ObservableCollection<Ticket>(Tickets.Where(t => t.State == "Active").OrderBy(t => t.Id));
        }

        private void LoadRates()
        {
            HourlyRate = 5.00m;
            HalfDayRate = 25.00m;
            FullDayRate = 40.00m;
            ProvincialTaxRate = 0.05m;
            FederalTaxRate = 0.09975m;
        }

        [RelayCommand]
        private void AddUser()
        {
            var createUserWindow = new CreateUser();
            createUserWindow.Owner = Application.Current.MainWindow;
            createUserWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (createUserWindow.ShowDialog() == true)
            {
                var viewModel = (CreateUserVM)createUserWindow.DataContext;

                Users.Add(new User(viewModel.Username, viewModel.Password, viewModel.FirstName, viewModel.LastName, viewModel.Email, viewModel.State));
            }
        }


        [RelayCommand]
        private Task EditUser(User user)
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task DeleteUser(User user)
        {
            if (user != null)
            {
                Users.Remove(user);
            }
            return Task.CompletedTask;
        }

        [RelayCommand]
        private void DeleteTicket(Ticket ticket)
        {
            if (ticket != null)
            {
                Tickets.Remove(ticket);
            }
        }

        [RelayCommand]
        private Task UpdateHourlyRate()
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task UpdateHalfDayRate()
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task UpdateFullDayRate()
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task UpdateProvincialTaxRate()
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task UpdateFederalTaxRate()
        {
            return Task.CompletedTask;
        }
    }
}
