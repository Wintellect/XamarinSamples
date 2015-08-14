using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UITestDemo
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate {};
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string addString;
        public string AddString
        {
            get
            {
                return addString;
            }
            set
            {
                if (addString != value)
                {
                    addString = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainPageViewModel()
        {
            this.AddString = "2 + 2 = ?";
        }

        public void UpdateButtonText()
        {
            var result = Add.AddItems(2, 2);

            this.AddString = "2 + 2 = " + result;
        }
    }
}
