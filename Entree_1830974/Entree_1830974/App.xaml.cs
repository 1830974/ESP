using Entree_1830974.Data.Context;
using Entree_1830974.ViewModels;
using Entree_1830974.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Entree_1830974
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

            services.AddTransient<MainWindowVM>(provider =>
            new MainWindowVM(
                provider.GetRequiredService<CiusssContext>(),
                provider.GetRequiredService<IConfiguration>())
            );
            services.AddTransient<MainWindow>();

            var serviceProvider = services.BuildServiceProvider();
            ServiceProvider = serviceProvider;

            CiusssContext context = serviceProvider.GetRequiredService<CiusssContext>();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();
        }
    }
}
