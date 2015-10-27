using Xamarin.Forms;

namespace PropertyDependencyDemo
{
    public class App : Application
    {
        public App ()
        {
            var viewModel = new MyViewModel ();
            var view = new MyPage ();
            view.BindingContext = viewModel;

            MainPage = new NavigationPage(view);
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}

