using Xamarin.Forms;
using System.Threading.Tasks;

namespace UITestDemo
{
    public class App : Application
    {
        INavigation _navigation;

        new public static App Current {
            get {
                return (App)Application.Current;
            }
        }

        public App()
        {
            var navRoot = new NavigationPage(new MainPage() { Title = "Main Page" });

            _navigation = navRoot.Navigation;

            MainPage = navRoot;
        }

        public async Task NavigateToDetailPage()
        {
            var page = new DetailPage();

            page.SetValue(NavigationPage.BackButtonTitleProperty, "Back");

            await _navigation.PushAsync(page);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
