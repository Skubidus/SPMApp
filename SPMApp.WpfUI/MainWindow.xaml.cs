using Microsoft.Extensions.Configuration;

using SPMApp.WpfUI.ViewModels;

using System.Windows;

namespace SPMApp.WpfUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IConfiguration _config;

    public MainWindowViewModel ViewModel { get; }

    public MainWindow(IConfiguration config, MainWindowViewModel viewModel)
    {
        _config = config;
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        (double windowWidth, double windowHeight, bool isWindowMaximized) = GetCustomWindowSettings();
        RestoreMainWindowSettings(windowWidth, windowHeight, isWindowMaximized);
    }

    private void RestoreMainWindowSettings(double windowWidth, double windowHeight, bool isWindowMaximized)
    {
        this.Width = windowWidth;
        this.Height = windowHeight;
        this.WindowState = isWindowMaximized
            ? WindowState.Maximized
            : WindowState.Normal;
    }

    private (int windowWidth, int windowHeight, bool isWindowMaximized) GetCustomWindowSettings()
    {
        var windowWidth = _config.GetValue<int?>("WindowWidth")
            ?? _config.GetValue<int?>("DefaultWindowWidth");

        var windowHeight = _config.GetValue<int?>("WindowHeight")
            ?? _config.GetValue<int?>("DefaultWindowHeight");

        var isWindowMaximized = _config.GetValue<bool?>("WindowMaximized")
            ?? _config.GetValue<bool?>("DefaultWindowMaximized");

        return (windowWidth!.Value, windowHeight!.Value, isWindowMaximized!.Value);
    }
}