using Xamarin.Forms;

namespace DisclosureAccessoryDemo.Xaml
{
    public static class CellExtensions
    {
        /// <summary>
        /// Determines the type of Cell Accessory to display. Default is None.
        /// </summary>
        public static BindableProperty AccessoryProperty = 
            BindableProperty.CreateAttached("Accessory", typeof(AccessoryType), typeof(Cell), AccessoryType.None);
    }
}
