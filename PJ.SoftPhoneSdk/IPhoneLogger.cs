namespace PJ.SoftPhoneSdk;

/// <summary>
/// 电话日志记录程序
/// </summary>
public interface IPhoneLogger
{
    /// <summary>
    /// 致命错误级别消息
    /// </summary>
    /// <param name="threadName">线程名称</param>
    /// <param name="message">消息内容</param>
    /// <param name="threadId">线程编号</param>
    void Fatal(int threadId, string threadName, string message);

    /// <summary>
    /// 普通错误级别消息
    /// </summary>
    /// <param name="threadName">线程名称</param>
    /// <param name="message">消息内容</param>
    /// <param name="threadId">线程编号</param>
    void Error(int threadId, string threadName, string message);

    /// <summary>
    /// 警告级别消息
    /// </summary>
    /// <param name="threadName">线程名称</param>
    /// <param name="message">消息内容</param>
    /// <param name="threadId">线程编号</param>
    void Warn(int threadId, string threadName, string message);

    /// <summary>
    /// 普通日志
    /// </summary>
    /// <param name="threadName">线程名称</param>
    /// <param name="message">消息内容</param>
    /// <param name="threadId">线程编号</param>
    void Info(int threadId, string threadName, string message);

    /// <summary>
    /// 调试日志
    /// </summary>
    /// <param name="threadName">线程名称</param>
    /// <param name="message">消息内容</param>
    /// <param name="threadId">线程编号</param>
    void Debug(int threadId, string threadName, string message);
}