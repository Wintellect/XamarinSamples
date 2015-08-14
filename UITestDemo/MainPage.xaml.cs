using System;
using Xamarin.Forms;

namespace UITestDemo
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel _mainViewModel;

        public MainPage()
        {
            InitializeComponent();

            _mainViewModel = new MainPageViewModel();

            BindingContext = _mainViewModel;
        }

        async void DetailClicked(object sender, EventArgs args)
        {
            await App.Current.NavigateToDetailPage();
        }

        void CalculateClicked(object sender, EventArgs args)
        {
            _mainViewModel.UpdateButtonText();
        }
    }
}
