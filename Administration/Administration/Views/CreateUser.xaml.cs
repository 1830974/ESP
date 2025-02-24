using Administration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Administration.Views
{
    /// <summary>
    /// Logique d'interaction pour CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            InitializeComponent();

            var viewModel = new CreateUserVM();

            viewModel.OnUserCreated = (user, password) =>
            {
                MessageBox.Show($"Utilisateur créé: {user.Username}", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            };

            viewModel.OnCancel = () =>
            {
                DialogResult = false;
                Close();
            };

            DataContext = viewModel;
        }
    }
}
