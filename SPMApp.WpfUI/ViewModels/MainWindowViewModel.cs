using CommunityToolkit.Mvvm.ComponentModel;

using SPMApp.WpfUI.Views;

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

        ViewController.RightViewChanged += ViewController_ViewChanged;
    }

    private void ViewController_ViewChanged(object? sender, UserControl newView)
    {
        CurrentViewRight = newView;
    }
}