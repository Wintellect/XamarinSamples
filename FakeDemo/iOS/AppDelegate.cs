using Foundation;
using UIKit;

namespace FakeDemo.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

			Xamarin.Calabash.Start();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
