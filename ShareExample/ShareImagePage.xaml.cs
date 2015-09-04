using Xamarin.Forms;

namespace ShareExample
{
    public partial class ShareImagePage : ContentPage
    {
        ShareImageViewModel _shareImageViewModel;

        public ShareImagePage()
        {
            InitializeComponent();

            _shareImageViewModel = new ShareImageViewModel();

            _shareImageViewModel.Source = LogoImage.Source;

            BindingContext = _shareImageViewModel;
        }
    }
}
