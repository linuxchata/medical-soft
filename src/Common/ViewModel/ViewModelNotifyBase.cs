using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Common.ViewModel
{
    /// <summary>
    /// Base class for view model which implement INotifyPropertyChanged interface.
    /// </summary>
    public abstract class ViewModelNotifyBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to raise PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged<T>(Expression<Func<T>> propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var memberExpression = (MemberExpression)propertyName.Body;
                this.PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
            }
        }
    }
}
