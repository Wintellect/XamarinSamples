using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace PropertyDependencyDemo.Mvvm
{
    /// <summary>
    /// Simple strong-typed property notification subscription mechanism.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public sealed class PropertyDependency<TObject, TProperty>
        where TObject : INotifyPropertyChanged
    {
        readonly List<Action> _actions;
        readonly List<Func<Task>> _tasks;
        readonly string _propertyName;

        public PropertyDependency(TObject source, string propertyName)
        {
            _actions = new List<Action>();
            _tasks = new List<Func<Task>>();
            _propertyName = propertyName;
            source.PropertyChanged += SourcePropertyChanged;
        }

        void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != _propertyName)
                return;
            _actions.ForEach(action => action());
            _tasks.ForEach(task => task().ConfigureAwait(true));
        }

        public PropertyDependency(TObject source, Expression<Func<TObject, TProperty>> propertyExpression)
            : this(source, propertyExpression.ExtractPropertyName())
        {
        }

        public PropertyDependency<TObject, TProperty> AlsoInvokeAction(Action action)
        {
            _actions.Add(action);
            return this;
        }

        public PropertyDependency<TObject, TProperty> AlsoInitiateTask(Func<Task> task)
        {
            _tasks.Add(task);
            return this;
        }
    }

    /// <summary>
    /// Tracks dependencies between properties of a bindable object.
    /// </summary>
    public sealed class PropertyDependency
    {
        readonly string _propertyName;
        readonly ObservableObject _observable;
        readonly Dictionary<string, List<string>> _propertyDependencies;
        readonly Dictionary<string, List<Action<ObservableObject, string>>> _propertyActions;
        readonly Dictionary<string, List<Func<Task>>> _propertyTasks;

        internal PropertyDependency(ObservableObject observable, string propertyName,
            Dictionary<string, List<string>> propertyDependencies,
            Dictionary<string, List<Action<ObservableObject, string>>> propertyActions,
            Dictionary<string, List<Func<Task>>> propertyTasks)
        {
            _observable = observable;
            _propertyName = propertyName;
            _propertyDependencies = propertyDependencies;
            _propertyActions = propertyActions;
            _propertyTasks = propertyTasks;
        }

        public PropertyDependency AlsoRaisePropertyChangedFor<T>(Expression<Func<T>> propertyExpression)
        {
            var dependant = propertyExpression.ExtractPropertyName();
            return AlsoRaisePropertyChangedFor(dependant);
        }

        public PropertyDependency AlsoRaisePropertyChangedFor(string propertyName)
        {
            if (!_observable.HasProperty(propertyName))
                throw new ArgumentException(
                    String.Format("Property '{0}' not found on type '{1}'.", propertyName,
                        _observable.GetType().Name), propertyName);
            if (!_propertyDependencies.ContainsKey(_propertyName))
                _propertyDependencies.Add(_propertyName, new List<string>());
            if (!_propertyDependencies[_propertyName].Contains(propertyName))
                _propertyDependencies[_propertyName].Add(propertyName);
            return this;
        }

        public PropertyDependency AlsoInvokeAction(Action<ObservableObject, string> action)
        {
            if (!_propertyActions.ContainsKey(_propertyName))
                _propertyActions.Add(_propertyName, new List<Action<ObservableObject, string>>());
            _propertyActions[_propertyName].Add(action);
            return this;
        }

        public PropertyDependency AlsoInvokeAction(Action action)
        {
            return AlsoInvokeAction((a, b) => action());
        }

        public PropertyDependency AlsoInitiateTask(Func<Task> task)
        {
            if (!_propertyTasks.ContainsKey(_propertyName))
                _propertyTasks.Add(_propertyName, new List<Func<Task>>());
            _propertyTasks[_propertyName].Add(task);
            return this;
        }
    }
}

