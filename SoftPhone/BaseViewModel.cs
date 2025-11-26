using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoftPhone
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly Hashtable _fields = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected T? Get<T>([CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return default;
            if (!_fields.ContainsKey(propertyName)) return default;
            var val = _fields[propertyName];
            if (val == null) return default;
            return (T)val;
        }

        protected void Set<T>(T value, [CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;
            if (!_fields.ContainsValue(propertyName))
            {
                _fields[propertyName] = value;
                OnPropertyChanged(propertyName);
                return;
            }

            var oldVal = _fields[propertyName];
            if (oldVal == null && value != null)
            {
                _fields[propertyName] = value;
                OnPropertyChanged(propertyName);
                return;
            }

            if (EqualityComparer<T>.Default.Equals((T)oldVal!, value)) return;
            _fields[propertyName] = value;
            OnPropertyChanged(propertyName);
        }
    }
}
