using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Administration.Ressources;
using Administration.Views;
using Microsoft.Extensions.DependencyInjection;
using Administration.Data.Context;
using System.Windows.Media.Animation;
using Administration.Models;
using Microsoft.EntityFrameworkCore;

namespace Administration.ViewModels
{
    public partial class LoginVM : ObservableObject
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private Visibility _errorMessageVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _passwordBoxVisibility = Visibility.Visible;

        [ObservableProperty]
        private Visibility _passwordTextBoxVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private string _passwordVisibilityIcon = "Eye";

        private readonly IServiceProvider _serviceProvider;
        private readonly CiusssContext _context;

        public LoginVM(IServiceProvider serviceProvider, CiusssContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ShowLoginError("Nom d'utilisateur et mot de passe requis.");
                return;
            }

            User? currentUser = await _context.Users.Where(u => u.Username == Username).FirstOrDefaultAsync();
            if (currentUser == null || !PasswordHelper.VerifyPassword(Password, currentUser.Password))
            {
                ShowLoginError("Informations de connexion invalide.");
                return;
            }

            var home = _serviceProvider.GetRequiredService<Home>();
            home.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            home.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = home;
        }

        [RelayCommand]
        private void TogglePasswordVisibility()
        {
            if (PasswordBoxVisibility == Visibility.Visible)
            {
                PasswordBoxVisibility = Visibility.Collapsed;
                PasswordTextBoxVisibility = Visibility.Visible;
                PasswordVisibilityIcon = "EyeOff";
            }
            else
            {
                PasswordBoxVisibility = Visibility.Visible;
                PasswordTextBoxVisibility = Visibility.Collapsed;
                PasswordVisibilityIcon = "Eye";
            }
        }

        private void ShowLoginError(string message)
        {
            ErrorMessage = message;
            ErrorMessageVisibility = Visibility.Visible;
        }
    }
}
