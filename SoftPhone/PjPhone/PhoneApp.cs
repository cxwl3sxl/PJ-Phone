using System;
using SoftPhone.Sip;

namespace SoftPhone.PjPhone
{
    internal partial class PhoneApp : IPhone
    {
        private SipAccount? _account;

        public PhoneApp()
        {
            Instances.Add(this);
        }


        public void Dispose()
        {
            Instances.Remove(this);
            if (_account == null) return;
            SipPhone.RemoveAccount(_account);
        }

        public void Login(string server, int port, string number, string password)
        {
            _account = SipPhone
                .AddSipAccount(number, password, server, port)
                .SetDisplayName(Guid.NewGuid().ToString());
        }

        public void Call(string number)
        {
            _account?.Call(number);
        }

        public void Hangup()
        {
            _account?.Hangup();
        }

        public void Pickup()
        {
          
        }

        public event RegistrationStateChanged? OnRegistrationStateChanged;
        public event IncomingCall? OnIncomingCall;
        public event CallConnected? OnCallConnected;
        public event CallHangup? OnCallHangup;
    }
}
