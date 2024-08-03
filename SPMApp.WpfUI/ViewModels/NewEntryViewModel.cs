using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SPMApp.WpfUI.ViewModels;
public partial class NewEntryViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    private readonly EntryModel? _emptyEntry = new();

    [ObservableProperty]
    private EntryModel? _entry;

    [ObservableProperty]
    private string _pageTitle = "New Entry";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateEntryButtonClickCommand))]
    private int _id;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateEntryButtonClickCommand))]
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
    [NotifyCanExecuteChangedFor(nameof(CreateEntryButtonClickCommand))]
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
    [NotifyCanExecuteChangedFor(nameof(CreateEntryButtonClickCommand))]
    private string? _password;

    partial void OnPasswordChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.Password = value ?? null;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateEntryButtonClickCommand))]
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
    [NotifyCanExecuteChangedFor(nameof(CreateEntryButtonClickCommand))]
    private string? _notes;

    partial void OnNotesChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.Notes = value ?? null;
    }

    public readonly ObservableCollection<TagModel> Tags = [];

    [ObservableProperty]
    private DateTime _dateCreated;

    [ObservableProperty]
    private DateTime _dateModified;

    public NewEntryViewModel(ISqLiteData db)
    {
        _db = db;
    }

    partial void OnEntryChanged(EntryModel? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        MapEntryModelToUIProperties(value);
    }

    private void MapEntryModelToUIProperties(EntryModel value)
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
    private void OnGoBackButtonClick()
    {
        if (Entry is null)
        {
            throw new InvalidOperationException("Entry can not be null here.");
        }

        if (Entry.EqualsWithoutId(_emptyEntry) == false)
        {
            var result = MessageBox.Show(
                "Discard everything and go back WITHOUT saving?",
                "Go Back?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        ViewController.ChangeViewTo(ViewsEnum.EntryListView, SideEnum.Right);
    }

    private bool CanClickCreateEntryButton() => Entry?.EqualsWithoutId(_emptyEntry) == false;

    [RelayCommand(CanExecute = nameof(CanClickCreateEntryButton))]
    private void OnCreateEntryButtonClick()
    {
        if (Entry is null)
        {
            throw new InvalidOperationException("Entry can not be null here.");
        }

        _db.InsertEntry(Entry);

        _ = MessageBox.Show(
            "Entry created.",
            "Entry Saved",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        ViewController.ChangeViewTo(ViewsEnum.EntryListView, SideEnum.Right);
    }
}