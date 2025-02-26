using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administration.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Administration.ViewModels
{
    public partial class CreateUserVM : ObservableObject
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string firstName;

        [ObservableProperty]
        private string lastName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private bool state;

        public Action<User, string> OnUserCreated { get; set; }
        public Action OnCancel { get; set; }

        public CreateUserVM()
        {
            Username = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            State = false;
        }

        [RelayCommand]
        private void Confirm(object parameter)
        {
            var passwordBox = parameter as System.Windows.Controls.PasswordBox;
            if (passwordBox == null)
            {
                System.Windows.MessageBox.Show("Password entry failure", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email))
            {
                System.Windows.MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var newUser = new User
            {
                Username = Username,
                Password = password,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                State = State
            };

            OnUserCreated?.Invoke(newUser, password);
        }

        [RelayCommand]
        private void Cancel()
        {
            OnCancel?.Invoke();
        }
    }
}
