using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Views;
using Paiement_1830974.ViewModels;
using Paiement_1830974.Resources;

namespace Paiement_1830974
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddTransient<AccueilVM>(provider =>
            new AccueilVM(
                provider.GetRequiredService<IConfiguration>(),
                provider.GetRequiredService<INavigationService>())
            );

            services.AddTransient<AmmountVM>(provider =>
            new AmmountVM(
                provider.GetRequiredService<IConfiguration>(),
                provider.GetRequiredService<INavigationService>())
            );

            services.AddTransient<Home>();
            services.AddTransient<Accueil>();
            services.AddTransient<Ammount>();

            var serviceProvider = services.BuildServiceProvider();
            ServiceProvider = serviceProvider;

            var mainWindow = serviceProvider.GetRequiredService<Home>();
            mainWindow.Show();
        }
    }
}
