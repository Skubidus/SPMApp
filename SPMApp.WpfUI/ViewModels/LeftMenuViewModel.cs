using CommunityToolkit.Mvvm.ComponentModel;

using SPMLibrary.Data;

namespace SPMApp.WpfUI.ViewModels;

public partial class LeftMenuViewModel : ObservableObject
{

    //public LeftMenuViewModel()
    //{

    //}

    [ObservableProperty]
    private string _pageTitle = "Left Menu";
}