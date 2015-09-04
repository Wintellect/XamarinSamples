using Xamarin.Forms;

namespace ShareExample
{
    public class App : Application
    {
        public App()
        {
            var navigationPage = new NavigationPage(new ShareImagePage() { Title = "Share Logo" });

            MainPage = navigationPage;
        }
    }
}
