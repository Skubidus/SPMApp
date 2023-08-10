using CommunityToolkit.Mvvm.ComponentModel;

using SPMLibrary.Data;

using System.Windows.Controls;

namespace SPMApp.WpfUI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    [ObservableProperty]
    private UserControl _currentViewLeft;

    [ObservableProperty]
    private UserControl _currentViewRight;

    public MainWindowViewModel(ISqLiteData db)
    {
        _db = db;

        CurrentViewLeft = ViewController.GetView(ViewsEnum.LeftMenuView);
        CurrentViewRight = ViewController.GetView(ViewsEnum.EntryListView);

        ViewController.LeftViewChanged += ViewController_LeftViewChanged;
        ViewController.RightViewChanged += ViewController_RightViewChanged;
    }

    private void ViewController_LeftViewChanged(object? sender, UserControl newView)
    {
        CurrentViewLeft = newView;
    }

    private void ViewController_RightViewChanged(object? sender, UserControl newView)
    {
        CurrentViewRight = newView;
    }
}