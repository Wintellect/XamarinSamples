using System;
using PropertyDependencyDemo.Mvvm;

namespace PropertyDependencyDemo
{
    public class MyViewModel : ObservableObject
    {
        string _firstName;
        string _lastName;
        bool _showLastNameFirst;

        public string FirstName {
            get { return _firstName; }
            set { SetPropertyValue (ref _firstName, value); }
        }

        public string LastName {
            get { return _lastName; }
            set { SetPropertyValue (ref _lastName, value); }
        }

        public string FullName {
            get { return ShowLastNameFirst ? String.Format ("{0}, {1}", _lastName, _firstName) : String.Format ("{0} {1}", _firstName, _lastName); }
        }

        public bool ShowLastNameFirst {
            get { return _showLastNameFirst; }
            set { SetPropertyValue (ref _showLastNameFirst, value); }
        }

        public string Initials {
            get { return (String.IsNullOrEmpty (FirstName) ? "" : FirstName.Substring (0, 1)) + (String.IsNullOrEmpty (LastName) ? "" : LastName.Substring (0, 1)); }
        }

        public DelegateCommand SaveCommand { get; private set; }

        public MyViewModel ()
        {
            FirstName = "Keith";
            LastName = "Rome";

            SaveCommand = new DelegateCommand (() => {
                // TODO: Save Data ...
            }, 
                () => !(String.IsNullOrEmpty (FirstName) || String.IsNullOrEmpty (LastName)));

            WhenPropertyChanges (() => FirstName)
                .AlsoRaisePropertyChangedFor (() => FullName)
                .AlsoRaisePropertyChangedFor (() => Initials)
                .AlsoInvokeAction (SaveCommand.ChangeCanExecute);
            WhenPropertyChanges (() => LastName)
                .AlsoRaisePropertyChangedFor (() => FullName)
                .AlsoRaisePropertyChangedFor (() => Initials)
                .AlsoInvokeAction (SaveCommand.ChangeCanExecute);
            WhenPropertyChanges (() => ShowLastNameFirst)
                .AlsoRaisePropertyChangedFor (() => FullName);
        }
    }
}

