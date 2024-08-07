using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace SPMApp.WpfUI.ViewModels;
public partial class NewEntryViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private EntryModel? _entry;

    private EntryModel? _originalEntry;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string _pageTitle = "New Entry";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private int _id;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _title = string.Empty;

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
    private string? _username = string.Empty;

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
    private string? _password = string.Empty;

    partial void OnPasswordChanged(string? value)
    {
        if (Entry is null)
        {
            return;
        }

        Entry.Password = value ?? null;
    }

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

    private readonly ObservableCollection<TagModel> _tags = [];
    public ObservableCollection<TagModel> Tags => _tags;

    [ObservableProperty]
    private string? _tagText = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesButtonClickCommand))]
    private string? _notes = string.Empty;

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

    public NewEntryViewModel(ISqLiteData db)
    {
        _db = db;
        _tags.CollectionChanged += _tags_CollectionChanged;
    }

    private void _tags_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Debug.WriteLine("_tags has changed!");
        OnPropertyChanged(nameof(Tags));
        OnPropertyChanged(nameof(CanClickSaveChangesButton));
        ((RelayCommand)SaveChangesButtonClickCommand).NotifyCanExecuteChanged();
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
    private void OnGoBackButtonClick()
    {
        if (Entry is null)
        {
            throw new InvalidOperationException("Entry can not be null here.");
        }

        if (Entry.EqualsWithoutId(_originalEntry) == false)
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

    private bool CanClickSaveChangesButton
    {
        get
        {
            if (Entry is null
            || _originalEntry is null)
            {
                return false;
            }

            var output = (Entry.Equals(_originalEntry) == false)
                      || (EntryModel.AreTagListsEqual(_originalEntry.Tags, Tags) == false);

            return output;
        }
    }

    [RelayCommand(CanExecute = nameof(CanClickSaveChangesButton))]
    private void OnSaveChangesButtonClick()
    {
        ArgumentNullException.ThrowIfNull(Entry);

        _db.InsertEntry(Entry);

        _ = MessageBox.Show(
        "Entry created.",
        "Entry Created",
        MessageBoxButton.OK,
        MessageBoxImage.Information);

        _originalEntry = Entry;

        OnGoBackButtonClick();
    }

    [RelayCommand]
    private void OnRemoveTag(TagModel tag)
    {
        if (Entry is null)
        {
            throw new InvalidOperationException("Entry can not be null here.");
        }

        Tags.Remove(tag);
        Entry.Tags.Remove(tag);
    }

    [RelayCommand]
    private void OnAddTag()
    {
        if (Entry is null)
        {
            throw new InvalidOperationException("Entry can not be null here.");
        }

        if (string.IsNullOrWhiteSpace(TagText))
        {
            return;
        }

        TagText = TagText.Replace(" ", "");

        var tag = new TagModel { Title = TagText.ToLower() };
        Tags.Add(tag);
        Entry.Tags.Add(tag);

        TagText = string.Empty;
    }
}