using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using ParkingApp_1830974.Data.Context;
using Microsoft.EntityFrameworkCore;
using ParkingApp_1830974.Views;
using ParkingApp_1830974.ViewModels;

namespace ParkingApp_1830974
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddDbContext<CiusssContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("Home"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("Home")),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure()
                ));

            services.AddSingleton<IConfiguration>(configuration);

            services.AddTransient<LicensePlateVM>(provider =>
                new LicensePlateVM(
                    provider.GetRequiredService<CiusssContext>(),
                    provider.GetRequiredService<IConfiguration>())
            );
            services.AddTransient<LicensePlateVM>();
            services.AddTransient<Home>();
            services.AddTransient<LoginVM>();
            services.AddTransient<Login>();
            services.AddTransient<LicenseList>();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<Login>();
            mainWindow.Show();
        }
    }
}
