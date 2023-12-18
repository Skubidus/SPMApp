using SPMApp.WpfUI.Views;

using SPMLibrary.Models;

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

    public static T GetView<T, U>(ViewsEnum newView, U? parameter)
        where T : class
        where U : class
    {
        ArgumentNullException.ThrowIfNull(parameter);

        if (typeof(T).IsSubclassOf(typeof(UserControl)) == false
            && typeof(T) != typeof(UserControl))
        {
            throw new ArgumentException($"Type '{typeof(T)}' is not a valid UserControl.");
        }

        var output = (GetView(newView)) as T;
        //if (output is null)
        //    throw new InvalidOperationException($"{nameof(output)} can not be null here.");

        if (typeof(T) == typeof(EntryView)
            && typeof(U) == typeof(EntryModel))
        {
            var entry = parameter as EntryModel;
            var entryView = output as EntryView;
            entryView!.Entry = entry ?? throw new NullReferenceException();
            output = entryView as T;
        }

        return output ?? throw new InvalidOperationException($"{nameof(output)} can not be null here."); ;
    }

    public static void ChangeViewTo(ViewsEnum newView, SideEnum side)
    {
        var view = GetView(newView);

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

    public static void ChangeViewTo<T, U>(ViewsEnum newView, SideEnum side, U? parameter)
    where T : class
    where U : class
    {
        T view = GetView<T, U>(newView, parameter);

        if (typeof(T).IsSubclassOf(typeof(UserControl)) == false
            && typeof(T) != typeof(UserControl))
        {
            throw new ArgumentException($"Type '{typeof(T)}' is not a valid UserControl.");
        }

        switch (side)
        {
            case SideEnum.Left:
                _currentLeftView = view as UserControl ?? throw new InvalidOperationException();
                LeftViewChanged?.Invoke(null, _currentLeftView);
                break;

            case SideEnum.Right:
                _currentRightView = view as UserControl ?? throw new InvalidOperationException();
                RightViewChanged?.Invoke(null, _currentRightView);
                break;

            default:
                throw new InvalidOperationException($"The side provided was invalid.");
        }
    }
}