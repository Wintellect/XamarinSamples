using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using System.Linq;
using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using ShareExample.iOS;

[assembly: ExportRenderer (typeof(Page), typeof(PatchedPageRenderer))]
[assembly: ExportRenderer (typeof(NavigationPage), typeof(SecondaryToolbarRenderer))]

namespace ShareExample.iOS
{
    public class SecondaryToolbarRenderer : NavigationRenderer
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

    public class PatchedPageRenderer : PageRenderer
    {
        UIToolbar _toolbar;
        List<ToolbarItem> _secondaryItems;
        Dictionary<UIBarButtonItem, ICommand> _buttonCommands = new Dictionary<UIBarButtonItem, ICommand> ();

        protected override void OnElementChanged (VisualElementChangedEventArgs e)
        {
            var page = e.NewElement as Page;
            if (page != null) {
                _secondaryItems = page.ToolbarItems.Where (i => i.Order == ToolbarItemOrder.Secondary).ToList ();
                _secondaryItems.ForEach (t => page.ToolbarItems.Remove (t));
            }
            base.OnElementChanged (e);
        }

        public override void ViewWillAppear (bool animated)
        {
            var badBar = View.Subviews.OfType<UIToolbar> ().FirstOrDefault (v => v.GetType () != typeof(UIToolbar));
            if (badBar != null) {
                badBar.RemoveFromSuperview ();
            }

            if (_secondaryItems != null && _secondaryItems.Count > 0) {
                var tools = new List<UIBarButtonItem> ();
                _buttonCommands.Clear ();
                foreach (var tool in _secondaryItems) {
                    #pragma warning disable 618
                    var systemItemName = tool.Name;
                    #pragma warning restore 618
                    UIBarButtonItem button;
                    UIBarButtonSystemItem systemItem;
                    button = Enum.TryParse<UIBarButtonSystemItem> (systemItemName, out systemItem) 
                        ? new UIBarButtonItem (systemItem, ToolClicked) 
                        : new UIBarButtonItem (tool.Text, UIBarButtonItemStyle.Plain, ToolClicked);
                    _buttonCommands.Add (button, tool.Command);
                    tools.Add (button);
                }

                NavigationController.SetToolbarHidden (false, animated);
                _toolbar = new UIToolbar (CGRect.Empty) { Items = tools.ToArray () };
                NavigationController.View.Add (_toolbar);
            }

            base.ViewWillAppear (animated);
        }

        void ToolClicked(object sender, EventArgs args)
        {
            var tool = sender as UIBarButtonItem;
            var command = _buttonCommands [tool];
            command.Execute (null);
        }

        public override void ViewWillDisappear (bool animated)
        {
            if (_toolbar != null) {
                NavigationController.SetToolbarHidden (true, animated);
                _toolbar.RemoveFromSuperview ();
                _toolbar = null;
            }
            base.ViewWillDisappear (animated);
        }
    }
}
