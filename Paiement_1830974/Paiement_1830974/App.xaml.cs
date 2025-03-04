using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Paiement_1830974.Views;
using Paiement_1830974.ViewModels;
using Paiement_1830974.Resources;
using Microsoft.EntityFrameworkCore;
using Paiement_1830974.Data.Context;

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
            services.AddDbContext<CiusssContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("Default"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("Default")),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure()
                ));

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

            services.AddTransient<NIPVM>(provider =>
            new NIPVM(
                provider.GetRequiredService<IConfiguration>(),
                provider.GetRequiredService<INavigationService>())
            );

            services.AddTransient<BankConfirmVM>(provider =>
            new BankConfirmVM(
                provider.GetRequiredService<CiusssContext>(),
                provider.GetRequiredService<IConfiguration>(),
                provider.GetRequiredService<INavigationService>())
            );

            services.AddTransient<RecieptVM>(provider =>
            new RecieptVM(
                provider.GetRequiredService<IConfiguration>(),
                provider.GetRequiredService<INavigationService>())
            );

            services.AddTransient<Home>();
            services.AddTransient<Accueil>();
            services.AddTransient<Ammount>();
            services.AddTransient<NIP>();
            services.AddTransient<BankConfirm>();
            services.AddTransient<Reciept>();

            var serviceProvider = services.BuildServiceProvider();
            ServiceProvider = serviceProvider;

            var mainWindow = serviceProvider.GetRequiredService<Home>();
            mainWindow.Show();
        }
    }
}
