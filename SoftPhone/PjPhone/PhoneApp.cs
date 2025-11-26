using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone.PjPhone
{
    internal class PhoneApp : IPhone
    {
        public void Dispose()
        {
            // TODO 在此释放托管资源
        }

        public async void Login(string server, int port, string number, string password)
        {
            await Task.Delay(3000);
            OnRegistrationStateChanged?.Invoke(true, "在线");
        }

        public async void Call(string number)
        {
            await Task.Delay(3000);
            OnCallConnected?.Invoke();
        }

        public async void Hangup()
        {
            OnCallHangup?.Invoke();
            await Task.Delay(5000);
            OnIncomingCall?.Invoke("13800138001");
        }

        public void Pickup()
        {
            OnCallConnected?.Invoke();
        }

        public event RegistrationStateChanged? OnRegistrationStateChanged;
        public event IncomingCall? OnIncomingCall;
        public event CallConnected? OnCallConnected;
        public event CallHangup? OnCallHangup;
    }
}
