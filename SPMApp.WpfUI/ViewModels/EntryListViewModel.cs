﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMApp.WpfUI.Views;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryListViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    public event EventHandler<bool>? EntriesFiltered;
    public event EventHandler<bool>? FilterCleared;

    [ObservableProperty]
    private string _pageTitle = "Entry List";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchButtonClickCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearSearchButtonClickCommand))]
    private string _searchText = "";

    private readonly List<EntryModel> _entryCache = [];

    [ObservableProperty]
    private int _entryCacheCount;

    private readonly ObservableCollection<EntryModel> _entries = [];
    public ObservableCollection<EntryModel> Entries => _entries;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClearSearchButtonClickCommand))]
    private bool _isFiltered = false;

    [ObservableProperty]
    private EntryModel? _selectedEntry = null;

    partial void OnSelectedEntryChanged(EntryModel? oldValue, EntryModel? newValue)
    {
        if (newValue is null)
        {
            return;
        }

        ViewController.ChangeViewTo<EntryView, EntryModel>(ViewsEnum.EntryView, SideEnum.Right, newValue);
    }

    public EntryListViewModel(ISqLiteData db)
    {
        _db = db;
        UpdateEntryCacheFromDb();
        GetAllEntriesFromCache();
        EntryViewModel.EntryDeleted += EntryViewModel_EntryDeleted;
    }

    private void EntryViewModel_EntryDeleted(object? sender, EventArgs e)
    {
        UpdateEntryCacheFromDb();
        GetAllEntriesFromCache();
    }

    private void UpdateEntryCacheFromDb()
    {
        List<EntryModel> entries = _db.GetAllEntries();

        _entryCache.Clear();

        entries.ForEach(e => { _entryCache.Add(e); });
        EntryCacheCount = _entryCache.Count;
    }

    public void GetAllEntriesFromCache(bool updateCache = false)
    {
        if (updateCache)
        {
            UpdateEntryCacheFromDb();
        }

        Entries.Clear();

        _entryCache.ForEach(e => { Entries.Add(e); });
    }

    private bool CanClickSearchButton => string.IsNullOrWhiteSpace(SearchText) == false
                                         && _entryCache.Any();

    [RelayCommand(CanExecute = nameof(CanClickSearchButton))]
    public void OnSearchButtonClick()
    {
        var filterList = CreateFilterListFromSearchString(SearchText);
        var filteredEntries = FilterEntryList(filterList);

        _entries.Clear();
        filteredEntries.ForEach(e => { _entries.Add(e); });
    }

    private bool CanClickClearSearchButton => IsFiltered;

    [RelayCommand(CanExecute = nameof(CanClickClearSearchButton))]
    public void OnClearSearchButtonClick()
    {
        ClearFilter();
    }

    [RelayCommand]
    public void OnNewEntryButtonClick()
    {
        ViewController.ChangeViewTo<NewEntryView, EntryModel>(ViewsEnum.NewEntryView, SideEnum.Right, new());
    }

    private void ClearFilter()
    {
        GetAllEntriesFromCache();

        SearchText = "";
        IsFiltered = false;

        FilterCleared?.Invoke(this, false);
    }

    private List<string> CreateFilterListFromSearchString(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            throw new ArgumentNullException(nameof(searchText), "Search text can not be null or whitespace.");
        }

        var output = new List<string>();

        output.AddRange(SearchText.Split(' '));

        return output;
    }

    private List<EntryModel> FilterEntryList(IEnumerable<string> filterList)
    {
        ArgumentNullException.ThrowIfNull(filterList);

        List<EntryModel> output = [];

        foreach (var filter in filterList)
        {
            var filteredEntries = _entryCache.Where(e => e.Title.Contains(filter)
                                                         || e.WebsiteUrl?.Contains(filter) == true
                                                         || e.Notes?.Contains(filter) == true
                                                         || e.Tags.Any(t => t.Title.Contains(filter)));

            if (filteredEntries.Any())
            {
                output.AddRange(filteredEntries);
            }
        }

        output = output.Distinct()
                       .OrderBy(x => x.Title)
                       .ToList();

        IsFiltered = true;
        EntriesFiltered?.Invoke(this, true);

        return output;
    }
}