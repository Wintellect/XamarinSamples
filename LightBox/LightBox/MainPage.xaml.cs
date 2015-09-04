using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace LightBox
{
    public class ImageInfo
    {
        readonly string _baseUrl;

        public ImageInfo(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public string Text {
            get {
                return String.Format(_baseUrl, 320, 480);
            }
        }

        public string Thumbnail {
            get {
                return String.Format(_baseUrl, 50, 50);
            }
        }
    }

    public partial class MainPage : ContentPage
    {
        public MainPage ()
        {
            InitializeComponent ();

            var images = new[] {
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/1"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/2"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/3"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/4"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/5"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/6"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/7"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/8"),
                new ImageInfo("http://lorempixel.com/{0}/{1}/abstract/9")
            };
            ImagesList.ItemsSource = images;
        }

        Image _lightbox;
        Rectangle _origBounds;

        async void RowTapped (object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ImageInfo;
            _lightbox = new Image {
                WidthRequest = 100,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = 0,
                Source = item.Text
            };
            _lightbox.GestureRecognizers.Add (new TapGestureRecognizer {
                NumberOfTapsRequired = 1,
                Command = new Command (CloseLightBox)
            });
            RootGrid.Children.Add (_lightbox);
            RootGrid.ForceLayout ();
            _origBounds = _lightbox.Bounds;
            var fadeIn = _lightbox.FadeTo (1.0);
            var zoomIn = _lightbox.LayoutTo (RootGrid.Bounds);
            await Task.WhenAll (new[] { fadeIn, zoomIn });
        }

        void CloseLightBox()
        {
            var fadeOut = _lightbox.FadeTo (0.0);
            var zoomOut = _lightbox.LayoutTo (_origBounds);
            Task.WhenAll (new[] { fadeOut, zoomOut }).ContinueWith (task => {
                Device.BeginInvokeOnMainThread (() => {
                    RootGrid.Children.Remove (_lightbox);
                    _lightbox = null;
                });
            });
        }
    }
}

