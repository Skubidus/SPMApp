﻿using CommunityToolkit.Mvvm.ComponentModel;
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

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _username;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _password;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _websiteUrl;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
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
        // TODO: implement OnSaveChangesButtonClick() 
        throw new NotImplementedException();
    }
}