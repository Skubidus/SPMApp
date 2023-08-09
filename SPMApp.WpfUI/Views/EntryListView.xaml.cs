using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SPMApp.WpfUI.ViewModels;

using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace SPMApp.WpfUI.Views;

/// <summary>
/// Interaction logic for EntryListView.xaml
/// </summary>
public partial class EntryListView : UserControl
{
    public EntryListViewModel ViewModel { get; }

#nullable disable
    public EntryListView()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        ViewModel = App.Services!.GetService<EntryListViewModel>()
            ?? throw new InvalidOperationException($"{nameof(ViewModel)} was null.");

        DataContext = ViewModel;

        InitializeComponent();

        ViewModel.EntriesFiltered += ViewModel_EntriesFiltered;
        ViewModel.FilterCleared += ViewModel_FilterCleared;

        SearchText.Focus();
    }
#nullable restore

    private void ViewModel_FilterCleared(object? sender, bool e)
    {
        SearchText.Clear();
        SearchText.Focus();
    }

    private void ViewModel_EntriesFiltered(object? sender, bool e)
    {
        SearchText.Focus();
    }
}
