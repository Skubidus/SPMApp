using SPMApp.WpfUI.Views;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace SPMApp.WpfUI;

public static class ViewController
{
    private static UserControl? _currentView;
    private static readonly Dictionary<ViewsEnum, Type> _viewDictionary = new();
    public static event EventHandler<UserControl>? ViewChanged;

    static ViewController()
    {
        _viewDictionary.Add(ViewsEnum.MainWindow, typeof(MainWindow));
        _viewDictionary.Add(ViewsEnum.LeftMenuView, typeof(LeftMenuView));
        _viewDictionary.Add(ViewsEnum.EntryListView, typeof(EntryListView));
        _viewDictionary.Add(ViewsEnum.EntryView, typeof(EntryView));
    }

    public static UserControl GetView(ViewsEnum newView)
    {
        return (UserControl?)App.Services!.GetService(_viewDictionary[newView])
            ?? throw new InvalidOperationException($"The service returned for {newView} was null.");
    }

    public static void ChangeViewTo(ViewsEnum newView)
    {
        var view = (UserControl?)App.Services!.GetService(_viewDictionary[newView])
            ?? throw new InvalidOperationException($"The service returned for {newView} was null.");

        _currentView = view;
        ViewChanged?.Invoke(null, view);
    }
}