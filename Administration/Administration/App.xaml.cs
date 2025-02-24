﻿using Administration.Data.Context;
using Administration.Models;
using Administration.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Administration.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Administration
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

            services.AddTransient<Home>();
            services.AddTransient<LoginVM>();
            services.AddTransient<Login>();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<Login>();
            mainWindow.Show();
        }
    }
}
