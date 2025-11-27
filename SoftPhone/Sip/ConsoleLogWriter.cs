using System;
using System.Diagnostics;
using pj;

namespace SoftPhone.Sip;

class ConsoleLogWriter : LogWriter
{
    public override void write(LogEntry entry)
    {
        /*
         *   --* Level 0 Display fatal error only.
             * Level 1 Display error messages and more severe verbosity level only.
             * Level 2 Display Warning messages and more severe verbosity level only.
             * Level 3 Info verbosity (normally used by applications).
             * Level 4 Important PJSIP events.
             * Level 5 Detailed PJSIP events.
             --* Level 6 Very detailed PJLIB events.
         */
        switch (entry.level)
        {
            case 0:
                WriteLogEntry(entry, "fatal", msg => Trace.TraceError(msg));
                break;
            case 1:
                WriteLogEntry(entry, "error", msg => Trace.TraceError(msg));
                break;
            case 2:
                WriteLogEntry(entry, "warn", msg => Trace.TraceWarning(msg));
                break;
            case 3:
                WriteLogEntry(entry, "info", msg => Trace.TraceInformation(msg));
                break;
            case 4:
            case 5:
            case 6:
                WriteLogEntry(entry, "debug", msg => Trace.WriteLine(msg));
                break;
        }
    }

    void WriteLogEntry(LogEntry entry, string level, Action<string> func)
    {
        func($"[{entry.threadId} {entry.threadName}] [{level}] {entry.msg}");
    }
}