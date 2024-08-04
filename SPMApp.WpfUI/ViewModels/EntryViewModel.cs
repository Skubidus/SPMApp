using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace SPMApp.WpfUI.ViewModels;

public partial class EntryViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    public static event EventHandler? EntryDeleted;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private EntryModel? _entry;

    private EntryModel? _originalEntry;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string _pageTitle = "Entry";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private int _id;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _title;

    partial void OnTitleChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.Title = value ?? null;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _username;

    partial void OnUsernameChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.Username = value ?? null;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _password;

    partial void OnPasswordChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.Password = value ?? null;
    }

    private readonly ObservableCollection<TagModel> _tags = [];
    public ObservableCollection<TagModel> Tags => _tags;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _websiteUrl;

    partial void OnWebsiteUrlChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.WebsiteUrl = value ?? null;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _notes;

    partial void OnNotesChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.Notes = value ?? null;
    }

    [ObservableProperty]
    private DateTime _dateCreated;

    [ObservableProperty]
    private DateTime _dateModified;

    public EntryViewModel(ISqLiteData db) => _db = db;

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
    private static void OnGoBackButtonClick()
    {
        ViewController.ChangeViewTo(ViewsEnum.EntryListView, SideEnum.Right);
    }

    [RelayCommand]
    private void OnDeleteEntryButtonClick()
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

    private bool CanClickSaveChangesButton => Entry?.Equals(_originalEntry) == false;

    [RelayCommand(CanExecute = nameof(CanClickSaveChangesButton))]
    private void OnSaveChangesButtonClick()
    {
        ArgumentNullException.ThrowIfNull(Entry);

        _db.UpdateEntry(Entry);
        MessageBox.Show("Changes saved!");
        OnGoBackButtonClick();
    }

    [RelayCommand]
    private void OnTagButtonClick(TagModel tag)
    {
        return;
    }
}