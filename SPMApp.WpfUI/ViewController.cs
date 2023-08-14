using SPMApp.WpfUI.Views;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace SPMApp.WpfUI;

public static class ViewController
{
    private static UserControl? _currentLeftView;
    private static UserControl? _currentRightView;
    private static readonly Dictionary<ViewsEnum, Type> _viewDictionary = new();
    public static event EventHandler<UserControl>? LeftViewChanged;
    public static event EventHandler<UserControl>? RightViewChanged;

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

    public static void ChangeViewTo(ViewsEnum newView, SideEnum side)
    {
        var view = (UserControl?)App.Services!.GetService(_viewDictionary[newView])
            ?? throw new InvalidOperationException($"The service returned for {newView} was null.");

        switch (side)
        {
            case SideEnum.Left:
                _currentLeftView = view;
                LeftViewChanged?.Invoke(null, view);
                break;

            case SideEnum.Right:
                _currentRightView = view;
                RightViewChanged?.Invoke(null, view);
                break;

            default:
                throw new InvalidOperationException($"The side provided was invalid.");
        }
    }
}