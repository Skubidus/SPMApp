using Microsoft.Extensions.DependencyInjection;

using SPMApp.WpfUI.ViewModels;

using SPMLibrary.Models;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SPMApp.WpfUI.Views;

/// <summary>
/// Interaction logic for NewEntryView.xaml
/// </summary>
public partial class NewEntryView : UserControl
{
    public NewEntryViewModel ViewModel { get; }

#nullable disable
    public NewEntryView()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        ViewModel = App.Services!.GetService<NewEntryViewModel>()
            ?? throw new InvalidOperationException($"{nameof(ViewModel)} was null.");

        DataContext = ViewModel;

        InitializeComponent();
    }
#nullable restore

    public EntryModel Entry
    {
        get { return (EntryModel)GetValue(EntryProperty); }
        set
        {
            SetValue(EntryProperty, value);
            ViewModel.Entry = value;
        }
    }

    // Using a DependencyProperty as the backing store for Entry-property.
    // This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EntryProperty =
        DependencyProperty.Register(
            $"{nameof(NewEntryView)}_{nameof(Entry)}",
            typeof(EntryModel),
            typeof(EntryView),
            new PropertyMetadata(default));
}
