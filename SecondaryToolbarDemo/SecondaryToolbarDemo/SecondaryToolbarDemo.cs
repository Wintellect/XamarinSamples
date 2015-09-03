using Xamarin.Forms;

namespace SecondaryToolbarDemo
{
    public class App : Application
    {
        public Command ShareCommand { get; private set; }
        
        public App ()
        {
            ShareCommand = new Command (OnShare);
            MainPage = new NavigationPage (new MainAppPage { BindingContext = this });
        }

        void OnShare()
        {
            MainPage.DisplayAlert ("Toolbar Demo", "Hello from the toolbar!", "WHATEV");
        }
    }
}

