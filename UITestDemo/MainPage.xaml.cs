using System;
using Xamarin.Forms;

namespace UITestDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void DetailClicked(object sender, EventArgs args)
        {
            await App.Current.NavigateToDetailPage();
        }

        void CalculateClicked(object sender, EventArgs args)
        {
            Add.AddItems(2, 2); 
        }
    }
}
