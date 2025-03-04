﻿using Paiement_1830974.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paiement_1830974.Views
{
    /// <summary>
    /// Logique d'interaction pour Reciept.xaml
    /// </summary>
    public partial class Reciept : Page
    {
        public Reciept(RecieptVM recieptVM)
        {
            InitializeComponent();
            this.DataContext = recieptVM;
        }
    }
}
