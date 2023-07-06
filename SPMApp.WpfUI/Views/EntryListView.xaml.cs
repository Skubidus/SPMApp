using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SPMApp.WpfUI.ViewModels;

using System.ComponentModel;
using System;
using System.Windows.Controls;
using System.Windows;

namespace SPMApp.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for EntryListView.xaml
    /// </summary>
    public partial class EntryListView : UserControl
    {
        private readonly IConfiguration _config;

        public EntryListViewModel ViewModel { get; }

        public EntryListView()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            _config = App.Services!.GetService<IConfiguration>()
                ?? throw new InvalidOperationException("Configuration was null.");

            ViewModel = App.Services!.GetService<EntryListViewModel>()
                ?? throw new InvalidOperationException($"{nameof(ViewModel)} was null.");

            DataContext = ViewModel;

            InitializeComponent();

            ViewModel.EntriesFiltered += ViewModel_EntriesFiltered;
            ViewModel.FilterCleared += ViewModel_FilterCleared;

            SearchText.Focus();
        }

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
}
