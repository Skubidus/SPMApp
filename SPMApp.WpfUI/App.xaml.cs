using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SPMApp.WpfUI.ViewModels;
using SPMLibrary.DataAccess;

using System.IO;
using System;
using System.Windows;
using SPMApp.WpfUI.Views;
using SPMLibrary.Data;

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
        ConfigureServices(services, config);
        Services = services.BuildServiceProvider();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(ServiceCollection services, IConfiguration config)
    {
        // Views and ViewModel services
        services.AddTransient<MainWindow>();
        services.AddTransient<MainWindowViewModel>();

        services.AddTransient<LeftMenuView>();
        services.AddTransient<LeftMenuViewModel>();

        services.AddTransient<EntryListView>();
        services.AddTransient<EntryListViewModel>();

        // Database services
        services.AddTransient<ISqLiteDataAccess, SqLiteDataAccess>();
        services.AddTransient<ISqLiteData, SqLiteData>();

        services.AddSingleton(config);
    }
}