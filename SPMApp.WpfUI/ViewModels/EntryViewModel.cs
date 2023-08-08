using CommunityToolkit.Mvvm.ComponentModel;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryViewModel : ObservableObject
{
    private EntryReturnStates _returnState = EntryReturnStates.Unchanged;

    [ObservableProperty]
    private Entry? _entry;

    [ObservableProperty]
    private string _pageTitle = "Entry";

    public EntryViewModel()
    {

    }

    public void SetEntry((Entry entry, EntryReturnStates entryState) entryTuple)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(entryTuple.entry));


    }
}