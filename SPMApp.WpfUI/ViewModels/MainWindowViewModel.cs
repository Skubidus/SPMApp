using CommunityToolkit.Mvvm.ComponentModel;

using SPMLibrary.Data;
using SPMLibrary.DataAccess;

namespace SPMApp.WpfUI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly SqLiteData _db;

    public MainWindowViewModel(SqLiteData db)
    {
        _db = db;
    }
}