using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParkingApp_1830974.Ressources
{
    /// <summary>
    /// Fournit des méthodes pour gérer les mots de passe, y compris la génération, le hachage et la vérification.
    /// </summary>
    public static class PasswordHelper
    {
        private static readonly char[] _chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        /// <summary>
        /// Propriété de dépendance qui lie un mot de passe à un contrôle <see cref="PasswordBox"/>.
        /// </summary>
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        /// <summary>
        /// Obtient le mot de passe lié à un objet de dépendance.
        /// </summary>
        /// <param name="d">L'objet de dépendance à partir duquel obtenir le mot de passe.</param>
        /// <returns>Le mot de passe lié.</returns>
        public static string GetBoundPassword(DependencyObject d)
        {
            return (string)d.GetValue(BoundPasswordProperty);
        }

        /// <summary>
        /// Définit le mot de passe lié à un objet de dépendance.
        /// </summary>
        /// <param name="d">L'objet de dépendance sur lequel définir le mot de passe.</param>
        /// <param name="value">Le nouveau mot de passe à définir.</param>
        public static void SetBoundPassword(DependencyObject d, string value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }

        /// <summary>
        /// Gestionnaire d'événements appelé lorsque le mot de passe lié change.
        /// </summary>
        /// <param name="d">L'objet de dépendance dont la propriété a changé.</param>
        /// <param name="e">Les arguments de changement de propriété.</param>
        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged; // Détache l'événement pour éviter les boucles
                UpdatePassword(passwordBox, (string)e.NewValue);
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged; // Réattache l'événement
            }
        }

        /// <summary>
        /// Gestionnaire d'événements pour mettre à jour le mot de passe lié lorsque le mot de passe change dans le contrôle.
        /// </summary>
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBoundPassword(passwordBox, passwordBox.Password);
            }
        }

        /// <summary>
        /// Met à jour le mot de passe du contrôle <see cref="PasswordBox"/> si nécessaire.
        /// </summary>
        private static void UpdatePassword(PasswordBox passwordBox, string newPassword)
        {
            if (passwordBox.Password != newPassword)
            {
                passwordBox.Password = newPassword;
            }
        }

        /// <summary>
        /// Génère un mot de passe aléatoire d'une longueur spécifiée.
        /// </summary>
        /// <param name="length">La longueur du mot de passe à générer. Par défaut, 8 caractères.</param>
        /// <returns>Un mot de passe aléatoire sous forme de chaîne.</returns>
        public static string GenerateRandomPassword(int length = 8)
        {
            var data = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data); // Remplit le tableau avec des octets aléatoires
            }

            char[] passwordChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                passwordChars[i] = _chars[data[i] % _chars.Length]; // Sélectionne un caractère aléatoire
            }

            return new string(passwordChars);
        }

        /// <summary>
        /// Hache un mot de passe en utilisant BCrypt.
        /// </summary>
        /// <param name="password">Le mot de passe à hacher.</param>
        /// <returns>Le hachage du mot de passe.</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Vérifie si un mot de passe donné correspond à un hachage stocké.
        /// </summary>
        /// <param name="inputPassword">Le mot de passe à vérifier.</param>
        /// <param name="storedHash">Le hachage du mot de passe stocké.</param>
        /// <returns>true si le mot de passe correspond au hachage ; sinon, false.</returns>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
    }
}
