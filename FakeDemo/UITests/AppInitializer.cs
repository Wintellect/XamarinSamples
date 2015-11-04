using Xamarin.UITest;

namespace FakeDemo.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp()
        {
            return ConfigureApp.Android.ApkFile("../../com.companyname.fakedemo.apk").StartApp();
        }
    }
}
