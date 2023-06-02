using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SPMApp.WpfUI.ViewModels;
using SPMLibrary.DataAccess;

using System.IO;
using System;
using System.Windows;

namespace SPMApp.WpfUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServiceProvider? Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        IConfiguration config = builder.Build();

        // configure services here
        #region Service configuration
        // Views and ViewModel services
        services.AddTransient<MainWindow>();
        services.AddTransient<MainWindowViewModel>();

        // Database services
        services.AddTransient<ISqLiteDataAccess, SqLiteDataAccess>();
        //services.AddTransient<IDatabaseData, SqLiteData>();

        services.AddSingleton(config);
        #endregion

        Services = services.BuildServiceProvider();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}