using System;
using System.Diagnostics;
using System.Threading;
using SoftPhone.Sip;

namespace SoftPhone.PjPhone
{
    internal class PhoneApp : IPhone
    {
        #region static

        static PhoneApp()
        {
            SipPhone.Init(Thread.CurrentThread, false);
        }

        #endregion


        private SipAccount? _account;

        #region IPhone

        public void Dispose()
        {
            if (_account == null) return;
            _account.OnRegistrationStateChanged -= _account_OnRegistrationStateChanged;
            _account.OnCallMissed -= _account_OnCallMissed;
            _account.OnCalling -= _account_OnCalling;
            _account.OnCallStateChanged -= _account_OnCallStateChanged;
            SipPhone.RemoveAccount(_account);
        }

        public void Login(string server, int port, string number, string password)
        {
            _account = SipPhone
                .AddSipAccount(number, password, server, port)
                .SetDisplayName(Guid.NewGuid().ToString());
            _account.OnRegistrationStateChanged += _account_OnRegistrationStateChanged;
            _account.OnCallMissed += _account_OnCallMissed;
            _account.OnCalling += _account_OnCalling;
            _account.OnCallStateChanged += _account_OnCallStateChanged;
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
            _account?.Pickup();
        }

        public event RegistrationStateChanged? OnRegistrationStateChanged;
        public event IncomingCall? OnIncomingCall;
        public event CallConnected? OnCallConnected;
        public event CallHangup? OnCallHangup;

        #endregion

        #region event handler

        private void _account_OnCallStateChanged(object? sender, SipCall e)
        {
            Trace.WriteLine($"呼叫状态变化 {e.CallId} {e.State}");
        }

        private void _account_OnCalling(object? sender, SipCall e)
        {
            if (e.Direction != CallDirection.InComing) return;
            OnIncomingCall?.Invoke(e.getInfo().remoteContact);
        }

        private void _account_OnCallMissed(object? sender, SipCall e)
        {

        }

        private void _account_OnRegistrationStateChanged(object? sender, string e)
        {
            OnRegistrationStateChanged?
                .Invoke(_account!.IsRegistrationOk, _account.IsRegistrationOk ? "在线" : e);
        }

        #endregion
    }
}
