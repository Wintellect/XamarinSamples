using UIKit;
using Xamarin.Forms;
using DisclosureAccessoryDemo.Xaml;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System.Collections.Generic;
using System;

[assembly: ExportRenderer(typeof(TextCell), 
    typeof(DisclosureAccessoryDemo.iOS.AccessorizedTextCellRenderer))]
[assembly: ExportRenderer(typeof(ImageCell), 
    typeof(DisclosureAccessoryDemo.iOS.AccessorizedImageCellRenderer))]
[assembly: ExportRenderer(typeof(ViewCell),
    typeof(DisclosureAccessoryDemo.iOS.AccessorizedViewCellRenderer))]

namespace DisclosureAccessoryDemo.iOS
{
    internal class CellAccessory
    {
        private static List<CellAccessory> _watchedCells =
            new List<CellAccessory>();

        internal static void Apply(Cell cell, UITableViewCell nativeCell)
        {
            CellAccessory watchedCell = null;

            // look to see if we are already tracking this xaml cell
            foreach (var item in _watchedCells.ToArray())
            {
                Cell cellRef;
                if (item._cell.TryGetTarget(out cellRef))
                {
                    if (cellRef == cell)
                    {
                        watchedCell = item;
                        break;
                    }
                }
                else
                {
                    // remove dead entry from list
                    _watchedCells.Remove(item);
                }
            }

            // if not already tracking, set up new entry and monitor for property changes
            if (watchedCell == null)
            {
                watchedCell = new CellAccessory { _cell = new WeakReference<Cell>(cell) };
                cell.PropertyChanged += watchedCell.CellPropertyChanged;
                _watchedCells.Add(watchedCell);
            }

            // update the target native cell of the tracked xaml cell
            watchedCell._nativeCell = new WeakReference<UITableViewCell>(nativeCell);

            // force immediate update of accessory type
            watchedCell.Reapply();
        }

        private WeakReference<Cell> _cell;
        private WeakReference<UITableViewCell> _nativeCell;

        private void CellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // update native accessory type when xaml property changes
            if (args.PropertyName == CellExtensions.AccessoryProperty.PropertyName)
                Reapply();
        }

        private void Reapply()
        {
            Cell cell;
            UITableViewCell nativeCell;

            if (!_cell.TryGetTarget(out cell))
            {
                // remove dead entry from list
                _watchedCells.Remove(this);
                return;
            }

            if (!_nativeCell.TryGetTarget(out nativeCell))
            {
                // if a property change fires for a dead native cell (but xaml cell isn't dead), then ignore it
                return;
            }

            var acc = (AccessoryType)cell.GetValue(CellExtensions.AccessoryProperty);

            switch (acc)
            {
                case AccessoryType.None:
                    nativeCell.Accessory = UITableViewCellAccessory.None;
                    if (nativeCell.AccessoryView == null)
                        nativeCell.AccessoryView = new UIView(new CGRect(0, 0, 20, 40));
                    return;
                case AccessoryType.Checkmark:
                    nativeCell.Accessory = UITableViewCellAccessory.Checkmark;
                    break;
                case AccessoryType.DisclosureIndicator:
                    nativeCell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    break;
                case AccessoryType.DetailButton:
                    nativeCell.Accessory = UITableViewCellAccessory.DetailButton;
                    break;
                case AccessoryType.DetailDisclosureButton:
                    nativeCell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
                    break;
            }

            if (nativeCell.AccessoryView != null)
                nativeCell.AccessoryView.Dispose();
            nativeCell.AccessoryView = null;
        }
    }

    public class AccessorizedTextCellRenderer : TextCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            CellAccessory.Apply(item, cell);
            return cell;
        }
    }

    public class AccessorizedImageCellRenderer : ImageCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            CellAccessory.Apply(item, cell);
            return cell;
        }
    }

    public class AccessorizedViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            CellAccessory.Apply(item, cell);
            return cell;
        }
    }
}