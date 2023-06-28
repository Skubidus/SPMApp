using CommunityToolkit.Mvvm.ComponentModel;

using SPMLibrary.Data;

namespace SPMApp.WpfUI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ISqLiteData _db;

    public MainWindowViewModel(ISqLiteData db)
    {
        _db = db;
    }
}