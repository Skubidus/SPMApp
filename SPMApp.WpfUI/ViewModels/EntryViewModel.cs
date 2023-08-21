using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;
using System.Diagnostics;
using System.Windows;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    [ObservableProperty]
    private EntryModel? _entry;

    private EntryModel? _originalEntry;

    [ObservableProperty]
    private string _pageTitle = "Entry";

    public EntryViewModel(ISqLiteData db)
    {
        _db = db;
    }

    partial void OnEntryChanged(EntryModel? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (_originalEntry is not null)
        {
            throw new Exception($"Reassigning of {nameof(Entry)} is not allowed.");
        }

        _originalEntry = value.Clone();

        Debug.Assert(_originalEntry.Equals(value), "Entries are NOT equal!");
    }

    [RelayCommand]
    public void OnGoBackButtonClick()
    {
        ViewController.ChangeViewTo(ViewsEnum.EntryListView, SideEnum.Right);
    }

    [RelayCommand]
    public void OnDeleteEntryButtonClick()
    {
        if (Entry is null)
        {
            throw new InvalidOperationException($"The entry '{nameof(Entry)}' can not be null.");
        }

        var result = MessageBox.Show(
            "Do you really want to delete the current entry?",
            "Delete Entry",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.No);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        _db.DeleteEntry(Entry.Id);

        _ = MessageBox.Show(
            "The entry has been deleted.",
            "Entry Deleted",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        throw new NotImplementedException();
        // TODO: change view back to the EntryList
    }

    public bool CanClickSaveChangesButton => Entry?.Equals(_originalEntry) == false;

    [RelayCommand(CanExecute = nameof(CanClickSaveChangesButton))]
    public void OnSaveChangesButtonClick()
    {

    }
}