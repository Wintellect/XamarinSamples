using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertyDependencyDemo.Mvvm
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Given a lambda expression that contains a single reference to a public property, retrieves the property's setter accessor.
        /// </summary>
        /// <typeparam name="TProperty">Data type of property</typeparam>
        /// <param name="propertyExpression">A lambda expression in the form of <code>() => PropertyName</code></param>
        /// <returns></returns>
        public static Action<object, TProperty> ExtractPropertySetter<TProperty>(this Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");

            var setMethod = property.SetMethod;

            if (setMethod == null)
                throw new ArgumentException("The referenced property does not have a set method.", "propertyExpression");

            if (setMethod.IsStatic)
                throw new ArgumentException("The referenced property is a static property.", "propertyExpression");

            Action<object, TProperty> action = (obj, val) => setMethod.Invoke(obj, new object[] { val });
            return action;
        }

        /// <summary>
        /// Extracts the property name from the property expression.
        /// 
        /// Implementation borrowed from Jounce MVVM framework.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpression">A lambda expression in the form of <code>() => PropertyName</code></param>
        /// <param name="failSilently">When 'true' causes this method to return null results instead of throwing 
        /// an exception whenever a problem occurs while probing the type information.</param>
        /// <returns>The property name</returns>
        public static string ExtractPropertyName<TObject, TProperty>(this Expression<Func<TObject, TProperty>> propertyExpression, bool failSilently = false)
        {
            if (propertyExpression == null)
            {
                if (failSilently)
                    return null;
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                if (failSilently)
                    return null;
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                if (failSilently)
                    return null;
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
            }

            var getMethod = property.GetMethod;

            if (getMethod == null)
            {
                // this shouldn't happen - the expression would reject the property before reaching this far
                if (failSilently)
                    return null;
                throw new ArgumentException("The referenced property does not have a get method.", "propertyExpression");
            }

            if (getMethod.IsStatic)
            {
                if (failSilently)
                    return null;
                throw new ArgumentException("The referenced property is a static property.", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Extracts the property name from the property expression.
        /// 
        /// Implementation borrowed from Jounce MVVM framework.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpression">A lambda expression in the form of <code>() => PropertyName</code></param>
        /// <param name="failSilently">When 'true' causes this method to return null results instead of throwing 
        /// an exception whenever a problem occurs while probing the type information.</param>
        /// <returns>The property name</returns>
        public static string ExtractPropertyName<T>(this Expression<Func<T>> propertyExpression, bool failSilently = false)
        {
            if (propertyExpression == null)
            {
                if (failSilently)
                    return null;
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                if (failSilently)
                    return null;
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                if (failSilently)
                    return null;
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
            }

            var getMethod = property.GetMethod;

            if (getMethod == null)
            {
                // this shouldn't happen - the expression would reject the property before reaching this far
                if (failSilently)
                    return null;
                throw new ArgumentException("The referenced property does not have a get method.", "propertyExpression");
            }

            if (getMethod.IsStatic)
            {
                if (failSilently)
                    return null;
                throw new ArgumentException("The referenced property is a static property.", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Inspects a type to see if it defines a property with the specified name and type.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="obj">An instance of an object being inspected, or a variable of the corresponding class type (can be null).</param>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns 'true' if the property is found.</returns>
        public static bool HasProperty<T>(this T obj, string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
                return false;

            var type = Equals(obj, default(T)) ? typeof(T) : obj.GetType();

            // Verify that the property name matches a realinstance property on this object.
            return type.GetRuntimeProperty(propertyName) != null;
        }
    }

}

