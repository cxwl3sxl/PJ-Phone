using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;

namespace SoftPhone
{
    public class PhoneViewModel : BaseViewModel
    {
        public PhoneViewModel(PhoneProfile profile, IPhone phone)
        {
            DisplayName = $"{profile.Name}({profile.Number})";
            Profile = profile;
            NumberCommand = new RelayCommand<string>(NumberClick);
        }

        public PhoneProfile Profile { get; }

        public void UpdateProfile(PhoneProfile profile)
        {
            Profile.Name = profile.Name;
            Profile.Number = profile.Number;
            Profile.Password = profile.Password;
            Profile.Server = profile.Server;
            Profile.Port = profile.Port;
            DisplayName = $"{profile.Name}({profile.Number})";
        }

        public bool IsOnline
        {
            get => Get<bool>();
            set => Set(value);
        }

        public string? LineStatus
        {
            get => Get<string>();
            set => Set(value);
        }

        public string? DisplayName
        {
            get => Get<string>();
            set => Set(value);
        }

        public string? PhoneStatus
        {
            get => Get<string>();
            set => Set(value);
        }

        public IBrush? PhoneStatusColor
        {
            get => Get<IBrush>();
            set => Set(value);
        }

        public ICommand NumberCommand { get; }

        void NumberClick(string? number)
        {
            PhoneStatus += number;
        }
    }
}
