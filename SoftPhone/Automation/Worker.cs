using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace SoftPhone.Automation
{
    internal class Worker(AutoGroupItem item, bool isCaller, PhoneViewModel? phone)
    {
        private bool _stop;
        private readonly CancellationTokenSource _stopTokenSource = new();

        public void Start()
        {
            if (phone == null) return;
            phone.IsRobot = true;
            phone.SourcePhone.OnIncomingCall += Phone_OnIncomingCall;
            phone.SourcePhone.OnCallHangup += Phone_OnCallHangup;
            if (!isCaller) return;
            if (string.IsNullOrWhiteSpace(item.TargetNumber)) return;
            Trace.WriteLine($"[Worker] 主叫 {item.Number} 正在呼叫 {item.TargetNumber}");
            phone.SourcePhone.Call(item.TargetNumber);
        }

        private void Phone_OnCallHangup()
        {
            if (_stop) return;
            if (!isCaller) return;
            Dispatcher.UIThread.Post(async () =>
            {
                //主叫挂断后延时
                Trace.WriteLine($"[Worker] 主叫 {item.Number} 通话挂断，延时{item.Delay}s");
                await Delay(item.Delay * 1000);
                //再次发起
                if (_stop) return;
                Trace.WriteLine($"[Worker] 主叫 {item.Number} 正在呼叫 {item.TargetNumber}");
                phone!.SourcePhone.Call(item.TargetNumber!);
            });
        }

        private void Phone_OnIncomingCall(string callerNumber)
        {
            if (_stop) return;

            Dispatcher.UIThread.Post(async () =>
            {
                if (isCaller)
                {
                    //主叫呼入直接挂断
                    phone!.SourcePhone.Hangup();
                    return;
                }
                if (_stop) return;

                //被叫，接听电话
                Trace.WriteLine($"[Worker] 被叫 {item.Number} 正在接听来电 {callerNumber}");
                phone!.SourcePhone.Pickup();
                //延时
                Trace.WriteLine($"[Worker] 被叫 {item.Number} 延时{item.Delay}s, 模拟通话");
                await Delay(item.Delay * 1000);
                //挂断
                Trace.WriteLine($"[Worker] 被叫 {item.Number} 挂断来电");
                phone!.SourcePhone.Hangup();
            });
        }

        public void Stop()
        {
            _stopTokenSource.Cancel(false);
            _stop = true;
            if (phone == null) return;
            phone.IsRobot = false;
            phone.SourcePhone.OnIncomingCall -= Phone_OnIncomingCall;
            phone.SourcePhone.OnCallHangup -= Phone_OnCallHangup;
        }

        async Task Delay(int time)
        {
            try
            {
                await Task.Delay(time, _stopTokenSource.Token);
            }
            catch
            {
                // ignored
            }
        }
    }
}
