using System.Diagnostics;

namespace PJ.SoftPhoneSdk.Sip;

internal class TracePhoneLogger : IPhoneLogger
{
    public void Fatal(int threadId, string threadName, string message)
    {
        Trace.TraceError($"[{threadId} {threadName}] [fatal] {message}");
    }

    public void Error(int threadId, string threadName, string message)
    {
        Trace.TraceError($"[{threadId} {threadName}] [error] {message}");
    }

    public void Warn(int threadId, string threadName, string message)
    {
        Trace.TraceWarning($"[{threadId} {threadName}] [warn] {message}");
    }

    public void Info(int threadId, string threadName, string message)
    {
        Trace.TraceInformation($"[{threadId} {threadName}] [info] {message}");
    }

    public void Debug(int threadId, string threadName, string message)
    {
        Trace.WriteLine($"[{threadId} {threadName}] [debug] {message}");
    }
}