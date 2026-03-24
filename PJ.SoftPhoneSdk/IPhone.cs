namespace PJ.SoftPhoneSdk
{
    /// <summary>
    /// 电话注册状态变更事件
    /// </summary>
    /// <param name="isOnline">是否在线</param>
    /// <param name="reason">本次变化原因</param>
    public delegate void RegistrationStateChanged(bool isOnline, string reason);

    /// <summary>
    /// 电话呼入时间
    /// </summary>
    /// <param name="callerNumber">来电号码</param>
    public delegate void IncomingCall(string callerNumber);

    /// <summary>
    /// 电话挂断事件
    /// </summary>
    public delegate void CallHangup();

    /// <summary>
    /// 电话接通事件
    /// </summary>
    public delegate void CallConnected();

    /// <summary>
    /// 软电话接口定义
    /// </summary>
    public interface IPhone : IDisposable
    {
        /// <summary>
        /// 登录到服务器
        /// </summary>
        /// <param name="server">服务器地址</param>
        /// <param name="port">端口号，默认值 5060</param>
        /// <param name="number">登录分机号</param>
        /// <param name="password">登录密码</param>
        void Login(string server, int port, string number, string password);

        /// <summary>
        /// 设置录音文件保存目录
        /// </summary>
        /// <param name="dir">文件存储目录</param>
        void SetRecordingFileDir(string dir);

        /// <summary>
        /// 当前电话名称
        /// </summary>
        string? Name { get; set; }

        /// <summary>
        /// 呼叫某个号码
        /// </summary>
        /// <param name="number">目标号码</param>
        void Call(string number);

        /// <summary>
        /// 在当前通话中播放录音文件
        /// </summary>
        /// <param name="audioFile">录音文件地址<br/>
        /// 格式要求：<br/>
        ///  - 采样率：16000 Hz<br/>
        ///  - 声道数：1（单声道）<br/>
        ///  - 编码：16-bit PCM<br/>
        ///  - 格式：标准 WAV 文件<br/>
        /// <code>ffmpeg -i input.mp3 -ar 16000 -ac 1 -f wav output.wav</code>
        /// </param>
        void Play(string audioFile);

        /// <summary>
        /// 挂断当前通话
        /// </summary>
        void Hangup();

        /// <summary>
        /// 接听当前来电
        /// </summary>
        void Pickup();

        /// <summary>
        /// 线路注册状态变化事件
        /// </summary>
        event RegistrationStateChanged OnRegistrationStateChanged;

        /// <summary>
        /// 来电事件
        /// </summary>
        event IncomingCall OnIncomingCall;

        /// <summary>
        /// 电话接通事件
        /// </summary>
        event CallConnected OnCallConnected;

        /// <summary>
        /// 电话挂断事件
        /// </summary>
        event CallHangup OnCallHangup;
    }
}
