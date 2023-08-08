using Microsoft.Extensions.DependencyInjection;

using SPMApp.WpfUI.ViewModels;

using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace SPMApp.WpfUI.Views;

/// <summary>
/// Interaction logic for EntryView.xaml
/// </summary>
public partial class EntryView : UserControl
{
    public EntryViewModel ViewModel { get; }

    public EntryView()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        ViewModel = App.Services!.GetService<EntryViewModel>()
            ?? throw new InvalidOperationException($"{nameof(ViewModel)} was null.");

        DataContext = ViewModel;

        InitializeComponent();
    }
}
