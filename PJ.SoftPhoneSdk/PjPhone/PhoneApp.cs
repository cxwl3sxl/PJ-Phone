using System.Diagnostics;
using System.Text.RegularExpressions;
using PJ.SoftPhoneSdk.Sip;

namespace PJ.SoftPhoneSdk.PjPhone;

/// <summary>
/// 基于pjsip-pjsua2-cs的电话实现
/// </summary>
public class PhoneApp : IPhone
{
    #region static


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="hasSoundDevice">设置当前设备是否有音频设备</param>
    public static void Init(bool hasSoundDevice)
    {
        SipPhone.Init(Thread.CurrentThread, !hasSoundDevice);
    }

    private static readonly Regex NumberRegex = new Regex("\"\\d+\"");

    #endregion

    private SipAccount? _account;
    private SipCall? _currentCall;

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
            .AddSipAccount(number, password, server, port);
        _account.OnRegistrationStateChanged += _account_OnRegistrationStateChanged;
        _account.OnCallMissed += _account_OnCallMissed;
        _account.OnCalling += _account_OnCalling;
        _account.OnCallStateChanged += _account_OnCallStateChanged;
    }

    public string? Name
    {
        get => _account?.Name;
        set => _account?.SetDisplayName(value!);
    }

    public void Call(string number)
    {
        _account?.Call(number);
    }

    public void Play(string audioFile)
    {
        _currentCall?.Play(audioFile);
    }

    public void Hangup()
    {
        _account?.Hangup();
        OnCallHangup?.Invoke();
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
        Trace.WriteLine($"call {e.CallId} status -> {e.State}");
        switch (e.State)
        {
            case CallState.Connected:
                _currentCall = e;
                OnCallConnected?.Invoke();
                break;
            case CallState.DisConnected:
                _currentCall = null;
                OnCallHangup?.Invoke();
                break;
        }
    }

    private void _account_OnCalling(object? sender, SipCall e)
    {
        if (e.Direction != CallDirection.InComing) return;
        var number = NumberRegex.Match(e.getInfo().remoteUri).Value;
        OnIncomingCall?.Invoke(number.Replace("\"", ""));
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