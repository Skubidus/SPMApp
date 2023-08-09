using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using SPMApp.WpfUI.ViewModels;

using SPMLibrary.Models;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SPMApp.WpfUI.Views;

/// <summary>
/// Interaction logic for EntryView.xaml
/// </summary>
public partial class EntryView : UserControl
{
    public EntryViewModel ViewModel { get; }

    //public EntryView()
    //{
    //    if (DesignerProperties.GetIsInDesignMode(this))
    //    {
    //        return;
    //    }

    //    ViewModel = App.Services!.GetService<EntryViewModel>()
    //        ?? throw new InvalidOperationException($"{nameof(ViewModel)} was null.");

    //    DataContext = ViewModel;

    //    InitializeComponent();
    //}

    //public int MyProperty
    //{
    //    get { return (int)GetValue(MyPropertyProperty); }
    //    set { SetValue(MyPropertyProperty, value); }
    //}

    //public static DependencyProperty MyPropertyProperty => myPropertyProperty;




    public EntryModel Entry
    {
        get { return (EntryModel)GetValue(EntryProperty); }
        set { SetValue(EntryProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EntryProperty =
        DependencyProperty.Register(
            nameof(Entry),
            typeof(EntryModel),
            typeof(EntryView),
            new PropertyMetadata(default));



    public EntryView(EntryViewModel vm)
    {
        ViewModel = vm;
        ViewModel.Entry = Entry;

        DataContext = ViewModel;

        InitializeComponent();
    }
}
