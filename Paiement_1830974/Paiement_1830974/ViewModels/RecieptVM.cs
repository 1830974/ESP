﻿using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paiement_1830974.ViewModels
{
    public partial class RecieptVM : ObservableObject
    {
        private readonly string _apiKey;
        private readonly INavigationService _navigationService;
        [ObservableProperty] private string _printMessage; 

        public RecieptVM(IConfiguration configuration, INavigationService navigationService)
        {
            _apiKey = configuration["ApiKey"];
            _navigationService = navigationService;
            PrintMessage = "Imprimer un reçu ?";
        }
    }
}
