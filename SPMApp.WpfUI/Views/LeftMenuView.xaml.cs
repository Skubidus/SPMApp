using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SPMApp.WpfUI.ViewModels;

using System;
using System.ComponentModel;
using System.Configuration.Internal;
using System.Windows.Controls;

namespace SPMApp.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for LeftMenuView.xaml
    /// </summary>
    public partial class LeftMenuView : UserControl
    {
        private readonly IConfiguration _config;

        public LeftMenuViewModel ViewModel { get; }

        public LeftMenuView(IConfiguration config, LeftMenuViewModel viewModel)
        {
            _config = config;
            ViewModel = viewModel;

            DataContext = ViewModel;

            InitializeComponent();
        }

#nullable disable
        public LeftMenuView()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            _config = App.Services!.GetService<IConfiguration>()
                ?? throw new InvalidOperationException("Configuration was null.");

            ViewModel = App.Services!.GetService<LeftMenuViewModel>()
                ?? throw new InvalidOperationException($"{nameof(ViewModel)} was null.");

            DataContext = ViewModel;

            InitializeComponent();
        }
#nullable restore
    }
}
