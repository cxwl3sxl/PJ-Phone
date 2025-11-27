using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SoftPhone.Sip;

namespace SoftPhone.PjPhone
{
    partial class PhoneApp
    {
        private static readonly List<PhoneApp> Instances = new();

        static PhoneApp()
        {
            SipPhone.Init(Thread.CurrentThread, false);
            SipPhone.ClearAccount();
            SipPhone.OnRegistrationStateChanged += SipPhone_OnRegistrationStateChanged;
            SipPhone.OnIncomingCall += SipPhone_OnIncomingCall;
            SipPhone.OnCallStateChanged += SipPhone_OnCallStateChanged;
        }

        private static void SipPhone_OnCallStateChanged(SipAccount account, SipCall call)
        {
            var target = Instances.FirstOrDefault(a => a._account?.Name == account.Name);
            if (target == null) return;

            Trace.WriteLine($"呼叫状态变化  {call.CallId} {call.State}");
        }

        private static void SipPhone_OnIncomingCall(SipAccount account, SipCall call)
        {
            var instance = Instances
                .FirstOrDefault(a => a._account?.Name == account.Name);
            if (instance == null)
            {
                call.Hangup();
                return;
            }

            instance.OnIncomingCall?.Invoke(call.getInfo().remoteContact);
        }

        private static void SipPhone_OnRegistrationStateChanged(SipAccount account, string message)
        {
            Instances
                .FirstOrDefault(a => a._account?.Name == account.Name)?
                .OnRegistrationStateChanged?
                .Invoke(account.IsRegistrationOk, account.IsRegistrationOk ? "在线" : message);
        }
    }
}
