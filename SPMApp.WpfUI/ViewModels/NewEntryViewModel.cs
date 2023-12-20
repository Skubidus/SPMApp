using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SPMLibrary.Data;
using SPMLibrary.Models;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SPMApp.WpfUI.ViewModels;
// TODO: modify this class - its a copy of EntryView
public partial class NewEntryViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    [ObservableProperty]
    private EntryModel? _entry;

    [ObservableProperty]
    private string _pageTitle = "New Entry";

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
    public void OnGoBackButtonClick()
    {
        ViewController.ChangeViewTo(ViewsEnum.EntryListView, SideEnum.Right);
    }
}