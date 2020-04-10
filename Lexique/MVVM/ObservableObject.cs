using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Lexique.WpfApp.MVVM
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [Obsolete("Use RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression) instead please. Sample: RaisePropertyChanged(()=> Property);")]
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (PropertyChanged == null)
                return;
            string propertyName = GetPropertyName(propertyExpression);
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected virtual bool Set<TProperty>(Expression<Func<TProperty>> propertyExpression, ref TProperty field, TProperty newValue, bool forceSetEvenIfIdenticalValue = false)
        {
            string propertyName = GetPropertyName(propertyExpression);

            if (!forceSetEvenIfIdenticalValue && EqualityComparer<TProperty>.Default.Equals(field, newValue))
                return false;

            string[] linkedProperties = GetLinkedProperties(propertyExpression).ToArray();

            field = newValue;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            foreach (string linkedPropertyName in linkedProperties)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(linkedPropertyName));

            return true;
        }

        protected static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyExpression);
            return propertyInfo?.Name;
        }

        protected static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            MemberExpression body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)propertyExpression.Body;
                body = ubody.Operand as MemberExpression;
                if (body == null)
                    throw new ArgumentException($"Expression '{propertyExpression}' refers to a method, not a property.");
            }

            PropertyInfo propInfo = body.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{propertyExpression}' refers to a field, not a property.");

            return propInfo;
        }

        protected static IEnumerable<string> GetLinkedProperties<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            try
            {
                PropertyInfo propertyInfo = GetPropertyInfo(propertyExpression);

                return propertyInfo.GetCustomAttributes<LinkedPropertyAttribute>().Select(x => x.PropertyName);
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
