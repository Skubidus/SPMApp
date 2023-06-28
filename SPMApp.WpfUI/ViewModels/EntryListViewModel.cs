using CommunityToolkit.Mvvm.ComponentModel;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System.Collections.ObjectModel;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryListViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    [ObservableProperty]
    private string _pageTitle = "Entry List";

    private readonly ObservableCollection<Entry> _entries = new();
    public ObservableCollection<Entry> Entrys => _entries;

    public EntryListViewModel(ISqLiteData db)
    {
        _db = db;
    }
}