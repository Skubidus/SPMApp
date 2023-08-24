using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    public event EventHandler? EntryDeleted;

    [ObservableProperty]
    private EntryModel? _entry;

    private EntryModel? _originalEntry;

    [ObservableProperty]
    private string _pageTitle = "Entry";

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private string? _websiteUrl;

    [ObservableProperty]
    private string? _notes;

    public readonly ObservableCollection<TagModel> Tags = new();

    [ObservableProperty]
    private DateTime _dateCreated;

    [ObservableProperty]
    private DateTime _dateModified;

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

        MapProperties(value);

        Debug.Assert(_originalEntry.Equals(value), "Entries are NOT equal!");
    }

    private void MapProperties(EntryModel value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Id = value.Id;
        Title = value.Title;
        Username = value.Username;
        Password = value.Password;
        Notes = value.Notes;
        WebsiteUrl = value.WebsiteUrl;
        Tags.Clear();
        value.Tags.ForEach(t => Tags.Add(t));
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

        _db.DeleteEntry(Entry);
        EntryDeleted?.Invoke(this, EventArgs.Empty);

        ViewController.ChangeViewTo(ViewsEnum.EntryListView, SideEnum.Right);

        _ = MessageBox.Show(
            "The entry has been deleted.",
            "Entry Deleted",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public bool CanClickSaveChangesButton => Entry?.Equals(_originalEntry) == false;

    [RelayCommand(CanExecute = nameof(CanClickSaveChangesButton))]
    public void OnSaveChangesButtonClick()
    {

    }
}