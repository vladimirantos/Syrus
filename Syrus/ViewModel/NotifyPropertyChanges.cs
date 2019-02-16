using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Syrus.ViewModel
{
    class NotifyPropertyChanges : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _propertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged 
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }

        /// <summary>
        /// Raised the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) 
            => _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Raised the PropertyChanged event
        /// </summary>
        /// <param name="propertyNames">The property names</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            if (propertyNames == null)
                throw new ArgumentNullException("Null propertyNames");
            foreach (string property in propertyNames)
                OnPropertyChanged(property);
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="currentValue">The current value of the property</param>
        /// <param name="newValue">The new value of the property</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c></returns>
        protected bool SetProperty<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(currentValue, newValue))
                return false;
            currentValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="currentValue">The current value of the property</param>
        /// <param name="newValue">The new value of the property</param>
        /// <param name="propertyNames">The names of all properties</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c></returns>
        protected bool SetProperty<T>(ref T currentValue, T newValue, params string[] propertyNames)
        {
            if (Equals(currentValue, newValue))
                return false;
            currentValue = newValue;
            OnPropertyChanged(propertyNames);
            return true;
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="currentValue">The current value of the property</param>
        /// <param name="newValue">The new value of the property</param>
        /// <param name="onChange">Raise method with old and new value params</param>
        /// <param name="propertyNames">Name of the property</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c></returns>
        protected bool SetProperty<T>(ref T currentValue, T newValue, Action<T, T> onChange, 
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(currentValue, newValue))
                return false;
            currentValue = newValue;
            OnPropertyChanged(propertyName);
            onChange.Invoke(currentValue, newValue);
            return true;
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="currentValue">The current value of the property</param>
        /// <param name="newValue">The new value of the property</param>
        /// <param name="onChange">Raise method with old and new value params</param>
        /// <param name="propertyNames">The names of all properties</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c></returns>
        protected bool SetProperty<T>(ref T currentValue, T newValue, Action<T, T> onChange, params string[] propertyNames)
        {
            if (Equals(currentValue, newValue))
                return false;
            currentValue = newValue;
            OnPropertyChanged(propertyNames);
            onChange.Invoke(currentValue, newValue);
            return true;
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed
        /// </summary>
        /// <param name="equal">A function which returns <c>true</c> if the property value has changed, otherwise <c>false</c></param>
        /// <param name="action">The action where the property is set</param>
        /// <param name="propertyNames">The property name</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c></returns>
        protected bool SetProperty<T>(Func<bool> equal, Action action, [CallerMemberName] string propertyName = null)
        {
            if (equal())
                return false;
            action();
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets the value of the property to the specified value if it has changed
        /// </summary>
        /// <param name="equal">A function which returns <c>true</c> if the property value has changed, otherwise <c>false</c></param>
        /// <param name="action">The action where the property is set</param>
        /// <param name="propertyNames">The property names</param>
        /// <returns><c>true</c> if the property was changed, otherwise <c>false</c></returns>
        protected bool SetProperty<T>(Func<bool> equal, Action action, params string[] propertyNames)
        {
            if (equal())
                return false;
            action();
            OnPropertyChanged(propertyNames);
            return true;
        }
    }
}
