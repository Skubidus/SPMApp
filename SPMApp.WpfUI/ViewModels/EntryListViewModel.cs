using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System.Collections.ObjectModel;
using System.Linq;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryListViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    [ObservableProperty]
    private string _pageTitle = "Entry List";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchButtonClickCommand))]
    private string? _searchText;

    private readonly ObservableCollection<Entry> _entries = new();
    public ObservableCollection<Entry> Entries => _entries;

    public EntryListViewModel(ISqLiteData db)
    {
        _db = db;
        GetAllEntries();
    }

    public void GetAllEntries()
    {
        var entries = _db.GetAllEntries();

        Entries.Clear();

        entries.ForEach(e => { Entries.Add(e); });
    }

    private bool CanClickSearchButton => string.IsNullOrWhiteSpace(SearchText) == false && Entries.Any();

    [RelayCommand(CanExecute = nameof(CanClickSearchButton))]
    public void OnSearchButtonClick()
    {

    }
}