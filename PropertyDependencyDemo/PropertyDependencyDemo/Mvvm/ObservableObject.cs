using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;

namespace PropertyDependencyDemo.Mvvm
{
    public class ObservableObject : INotifyPropertyChanged
    {
        readonly Dictionary<string, List<string>> _propertyDependencies = new Dictionary<string, List<string>> ();
        readonly Dictionary<string, List<Action<ObservableObject, string>>> _propertyActions = new Dictionary<string, List<Action<ObservableObject, string>>> ();
        readonly Dictionary<string, List<Func<Task>>> _propertyTasks = new Dictionary<string, List<Func<Task>>> ();

        public ObservableObject ()
        {
            _propertyDependencies = new Dictionary<string, List<string>> ();
            _propertyActions = new Dictionary<string, List<Action<ObservableObject, string>>> ();
            _propertyTasks = new Dictionary<string, List<Func<Task>>> ();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        /// <summary>
        /// Allows for chaining multiple dependant property update notifications.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        protected PropertyDependency WhenPropertyChanges<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = propertyExpression.ExtractPropertyName();
            return new PropertyDependency(this, propertyName, _propertyDependencies, _propertyActions, _propertyTasks);
        }

        protected virtual void AfterPropertyChange(string propertyName)
        {
        }

        /// <summary>
        /// This is used to set a specific value for a property.
        /// </summary>
        /// <typeparam name="T">Type to set</typeparam>
        /// <param name="storageField">Storage field</param>
        /// <param name="newValue">New value</param>
        /// <param name="propExpr">Property expression</param>
        /// <returns>True if the property value was changed and an INotifyPropertyChanged was raised.</returns>
        protected bool SetPropertyValue<T>(ref T storageField, T newValue, Expression<Func<T>> propExpr)
        {
            if (Equals(storageField, newValue))
                return false;
            storageField = newValue;
            var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            RaisePropertyChanged(prop.Name);
            return true;
        }

        /// <summary>
        /// This is used to set a specific value for a property.
        /// </summary>
        /// <typeparam name="T">Type to set</typeparam>
        /// <param name="storageField">Storage field</param>
        /// <param name="newValue">New value</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>True if the property value was changed and an INotifyPropertyChanged was raised.</returns>
        protected bool SetPropertyValue<T>(ref T storageField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storageField, newValue))
                return false;
            storageField = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedCore(propertyName);

            if (_propertyActions.ContainsKey(propertyName))
                _propertyActions[propertyName].ForEach(action => action(this, propertyName));

            if (_propertyTasks.ContainsKey(propertyName))
                _propertyTasks[propertyName].ForEach(task => task().ConfigureAwait(true));

            if (!_propertyDependencies.ContainsKey(propertyName))
                return;

            var depList = new List<string>();
            var tmp = new Queue<string>(_propertyDependencies[propertyName]);
            while (tmp.Any())
            {
                var dep = tmp.Dequeue();
                if (!depList.Contains(dep))
                    depList.Add(dep);
                if (_propertyDependencies.ContainsKey(dep))
                    _propertyDependencies[dep].ForEach(tmp.Enqueue);
            }

            depList.ForEach(PropertyChangedCore);
        }

        void PropertyChangedCore(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
            AfterPropertyChange(propertyName);
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = propertyExpression.ExtractPropertyName();
            RaisePropertyChanged(propertyName);
        }
    }
}

