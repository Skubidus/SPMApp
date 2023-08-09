using CommunityToolkit.Mvvm.ComponentModel;

using SPMLibrary.Models;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryViewModel : ObservableObject
{
    [ObservableProperty]
    private EntryModel? _entry;

    [ObservableProperty]
    private string _pageTitle = "Entry";
}