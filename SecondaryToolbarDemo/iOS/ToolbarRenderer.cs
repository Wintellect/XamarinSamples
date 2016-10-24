using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using System.Linq;
using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Windows.Input;

[assembly: ExportRenderer (typeof(Page), typeof(SecondaryToolbarDemo.iOS.ToolbarRenderer))]

namespace SecondaryToolbarDemo.iOS
{
    public class ToolbarRenderer : PageRenderer
    {
        UIToolbar _toolbar;
        List<ToolbarItem> _secondaryItems;
        readonly Dictionary<UIBarButtonItem, ToolbarItem> _buttonCommands = new Dictionary<UIBarButtonItem, ToolbarItem> ();

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
                    _buttonCommands.Add (button, tool);
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
            var command = _buttonCommands[tool].Command;
            command.Execute(_buttonCommands[tool].CommandParameter);
        }

        public override void ViewWillDisappear (bool animated)
        {
            if (_toolbar != null) {
                NavigationController.SetToolbarHidden (true, animated);
                _toolbar.RemoveFromSuperview ();
                _toolbar = null;
                _buttonCommands.Clear ();
            }
            base.ViewWillDisappear (animated);
        }
    }
}

