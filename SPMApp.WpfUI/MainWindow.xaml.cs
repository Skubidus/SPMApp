﻿using Microsoft.Extensions.Configuration;

using SPMApp.WpfUI.ViewModels;

using System;
using System.Reflection;
using System.Windows;

namespace SPMApp.WpfUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IConfiguration _config;

    public MainWindowViewModel ViewModel { get; }

    public MainWindow(IConfiguration config, MainWindowViewModel vm)
    {
        _config = config;
        ViewModel = vm;
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

    private void SaveMainWindowSettings(double windowWidth, double windowHeight, WindowState windowState)
    {
        // TODO: Find out how to write values back to the _config (appsettings.json)
        throw new NotImplementedException();
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

    private void Window_Closed(object sender, EventArgs e)
    {
        SaveMainWindowSettings(this.Width, this.Height, this.WindowState);
    }
}