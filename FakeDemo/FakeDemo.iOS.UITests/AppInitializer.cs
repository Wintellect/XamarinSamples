using Xamarin.UITest;

namespace FakeDemo.iOS.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp()
        {
            return ConfigureApp.iOS.AppBundle("../../../iOS/iPhone/ReleaseFakeDemo.iOS.app").StartApp();
        }
    }
}
