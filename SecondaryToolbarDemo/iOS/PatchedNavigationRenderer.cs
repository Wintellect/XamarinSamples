using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using System.Linq;
using CoreGraphics;
using System;

[assembly: ExportRenderer (typeof(NavigationPage), typeof(SecondaryToolbarDemo.iOS.PatchedNavigationRenderer))]

namespace SecondaryToolbarDemo.iOS
{
    public class PatchedNavigationRenderer : NavigationRenderer
    {
        public override void ViewWillAppear (bool animated)
        {
            var badBar = View.Subviews.OfType<UIToolbar> ().FirstOrDefault (v => v.GetType () != typeof(UIToolbar));
            if (badBar != null) {
                badBar.RemoveFromSuperview ();
            }
            base.ViewWillAppear (animated);
        }

        public override void ViewDidLayoutSubviews ()
        {
            base.ViewDidLayoutSubviews ();

            UIView[] subviews = View.Subviews.Where (v => v != NavigationBar).ToArray ();
            var toolBarViews = subviews.Where (v => v is UIToolbar).ToArray ();
            var otherViews = subviews.Where (v => !(v is UIToolbar)).ToArray ();

            nfloat toolbarHeight = 0;

            foreach (var uIView in toolBarViews) {
                uIView.SizeToFit ();
                uIView.Frame = new CGRect {
                    X = 0,
                    Y = View.Bounds.Height - uIView.Frame.Height,
                    Width = View.Bounds.Width,
                    Height = uIView.Frame.Height,
                };
                var thisToolbarHeight = uIView.Frame.Height;
                if (toolbarHeight < thisToolbarHeight) {
                    toolbarHeight = thisToolbarHeight;
                }
            }

            var othersHeight = View.Bounds.Height - toolbarHeight;
            var othersFrame = new CGRect (View.Bounds.X, View.Bounds.Y, View.Bounds.Width, othersHeight);

            foreach (var uIView in otherViews) {
                uIView.Frame = othersFrame;
            }
        }
    }
    
}
