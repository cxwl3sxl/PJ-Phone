using System;

namespace SoftPhone
{
    public delegate void RegistrationStateChanged(bool isOnline, string reason);

    public delegate void IncomingCall(string callerNumber);

    public delegate void CallHangup();

    public delegate void CallConnected();

    public interface IPhone : IDisposable
    {
        void Login(string server, int port, string number, string password);

        void Call(string number);

        void Hangup();

        void Pickup();

        event RegistrationStateChanged OnRegistrationStateChanged;

        event IncomingCall OnIncomingCall;

        event CallConnected OnCallConnected;

        event CallHangup OnCallHangup;
    }
}
