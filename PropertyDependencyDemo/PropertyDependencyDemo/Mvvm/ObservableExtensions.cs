using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace PropertyDependencyDemo.Mvvm
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// Monitors an observable object for property change events.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source">The object being monitored</param>
        /// <param name="propertyExpression">A lambda expression in the form of <code>(obj) => obj.PropertyName</code></param>
        /// <returns></returns>
        public static PropertyDependency<TObject, TProperty> WhenPropertyChanges<TObject, TProperty>(this TObject source, Expression<Func<TObject, TProperty>> propertyExpression)
            where TObject : INotifyPropertyChanged
        {
            return new PropertyDependency<TObject, TProperty>(source, propertyExpression);
        }
    }
}

