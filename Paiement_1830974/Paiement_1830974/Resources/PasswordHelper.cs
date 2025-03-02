using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paiement_1830974.Resources
{
    /// <summary>
    /// Fournit des méthodes pour gérer les mots de passe, y compris la génération, le hachage et la vérification.
    /// </summary>
    public static class PasswordHelper
    {
        public static readonly DependencyProperty BoundNipProperty =
       DependencyProperty.RegisterAttached("BoundNip", typeof(int), typeof(PasswordHelper),
           new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundNipChanged));

        public static int GetBoundNip(DependencyObject d) => (int)d.GetValue(BoundNipProperty);

        public static void SetBoundNip(DependencyObject d, int value)
        {
            Debug.WriteLine($"SetBoundNip called with value: {value}");
            d.SetValue(BoundNipProperty, value);
        }

        private static void OnBoundNipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine($"OnBoundNipChanged called. Old: {e.OldValue}, New: {e.NewValue}");
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                UpdatePassword(passwordBox, (int)e.NewValue);
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("PasswordBox_PasswordChanged called");
            if (sender is PasswordBox passwordBox)
            {
                if (int.TryParse(passwordBox.Password, out int nip))
                {
                    Debug.WriteLine($"Parsed NIP: {nip}");
                    SetBoundNip(passwordBox, nip);
                }
                else
                {
                    Debug.WriteLine("Failed to parse NIP");
                }
            }
        }

        private static void UpdatePassword(PasswordBox passwordBox, int newNip)
        {
            Debug.WriteLine($"UpdatePassword called with newNip: {newNip}");
            if (passwordBox.Password != newNip.ToString())
            {
                passwordBox.Password = newNip.ToString();
            }
        }

        public static void AttachBoundNip(PasswordBox passwordBox)
        {
            Debug.WriteLine("AttachBoundNip called");
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
    }
}
