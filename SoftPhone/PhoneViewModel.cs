using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace SoftPhone
{
    public class PhoneViewModel : BaseViewModel, IDisposable
    {
        public PhoneViewModel(PhoneProfile profile)
        {
            Profile = profile;
            NumberCommand = new RelayCommand<string>(NumberClick);
            CloseOrOpenCommand = new RelayCommand(CloseOrOpen);
            ActionCommand = new RelayCommand(Action);
        }

        public PhoneProfile Profile { get; }

        public void UpdateProfile(PhoneProfile profile)
        {
            Profile.Name = profile.Name;
            Profile.Number = profile.Number;
            Profile.Password = profile.Password;
            Profile.Server = profile.Server;
            Profile.Port = profile.Port;
        }

        #region props

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

        public string? PhoneStatus
        {
            get => Get<string>();
            set => Set(value);
        }

        public string? ActionLabel
        {
            get => Get<string>();
            set => Set(value);
        }

        public IBrush? PhoneStatusColor
        {
            get => Get<IBrush>();
            set => Set(value);
        }

        #endregion

        #region NumberClick

        public ICommand NumberCommand { get; }

        void NumberClick(string? number)
        {
            PhoneStatus += number;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            // TODO 在此释放托管资源
        }

        #endregion

        #region CloseOrOpenCommand

        public ICommand CloseOrOpenCommand { get; }

        void CloseOrOpen()
        {
            IsOnline = !IsOnline;
        }

        #endregion

        #region ActionCommand

        public ICommand ActionCommand { get; }

        void Action()
        {
       
        }

        #endregion
    }
}
