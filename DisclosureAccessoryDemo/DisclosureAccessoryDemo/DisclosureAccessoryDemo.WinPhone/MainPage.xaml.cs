using Microsoft.Phone.Controls;

namespace DisclosureAccessoryDemo.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
            ThemeManager.ToLightTheme();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new DisclosureAccessoryDemo.App());
        }
    }
}
