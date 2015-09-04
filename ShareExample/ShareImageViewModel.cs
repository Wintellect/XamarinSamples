using Xamarin.Forms;

namespace ShareExample
{
    public class ShareImageViewModel
    {
        public Command Share { get; set; }

        public ImageSource Source { get; set; }

        public ShareImageViewModel()
        {
            Share = new Command(ShareCommand);
        }

        void ShareCommand()
        {
            MessagingCenter.Send<ImageSource>(this.Source, "Share");
        }
    }
}
