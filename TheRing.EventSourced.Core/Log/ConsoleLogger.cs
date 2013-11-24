namespace TheRing.EventSourced.Core.Log
{
    #region using

    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;

    #endregion

    public class ConsoleLogger : ILogger
    {
        #region Static Fields

        private static readonly int ProcessId = Process.GetCurrentProcess().Id;

        #endregion

        #region Constructors and Destructors

        public ConsoleLogger(string name)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void Debug(string text)
        {
            Console.WriteLine(this.Log("DEBUG", text));
        }

        public void Debug(string format, params object[] args)
        {
            Console.WriteLine(this.Log("DEBUG", format, args));
        }

        public void DebugException(Exception exc, string format)
        {
            Console.WriteLine(this.Log("DEBUG", exc, format));
        }

        public void DebugException(Exception exc, string format, params object[] args)
        {
            Console.WriteLine(this.Log("DEBUG", exc, format, args));
        }

        public void Error(string text)
        {
            Console.WriteLine(this.Log("ERROR", text));
        }

        public void Error(string format, params object[] args)
        {
            Console.WriteLine(this.Log("ERROR", format, args));
        }

        public void ErrorException(Exception exc, string format)
        {
            Console.WriteLine(this.Log("ERROR", exc, format));
        }

        public void ErrorException(Exception exc, string format, params object[] args)
        {
            Console.WriteLine(this.Log("ERROR", exc, format, args));
        }

        public void Fatal(string text)
        {
            Console.WriteLine(this.Log("FATAL", text));
        }

        public void Fatal(string format, params object[] args)
        {
            Console.WriteLine(this.Log("FATAL", format, args));
        }

        public void FatalException(Exception exc, string format)
        {
            Console.WriteLine(this.Log("FATAL", exc, format));
        }

        public void FatalException(Exception exc, string format, params object[] args)
        {
            Console.WriteLine(this.Log("FATAL", exc, format, args));
        }

        public void Info(string text)
        {
            Console.WriteLine(this.Log("INFO ", text));
        }

        public void Info(string format, params object[] args)
        {
            Console.WriteLine(this.Log("INFO ", format, args));
        }

        public void InfoException(Exception exc, string format)
        {
            Console.WriteLine(this.Log("INFO ", exc, format));
        }

        public void InfoException(Exception exc, string format, params object[] args)
        {
            Console.WriteLine(this.Log("INFO ", exc, format, args));
        }

        public void Warn(string text)
        {
            Console.WriteLine(this.Log("WARN ", text));
        }

        public void Warn(string format, params object[] args)
        {
            Console.WriteLine(this.Log("WARN ", format, args));
        }

        public void WarnException(Exception exc, string format)
        {
            Console.WriteLine(this.Log("WARN ", exc, format));
        }

        public void WarnException(Exception exc, string format, params object[] args)
        {
            Console.WriteLine(this.Log("WARN ", exc, format, args));
        }

        public void Trace(string text)
        {
            Console.WriteLine(this.Log("TRACE", text));
        }

        public void Trace(string format, params object[] args)
        {
            Console.WriteLine(this.Log("TRACE", format, args));
        }

        public void TraceException(Exception exc, string format)
        {
            Console.WriteLine(this.Log("TRACE", exc, format));
        }

        public void TraceException(Exception exc, string format, params object[] args)
        {
            Console.WriteLine(this.Log("TRACE", exc, format, args));
        }

        #endregion

        #region Methods

        private string Log(string level, string format, params object[] args)
        {
            return string.Format(
                "[{0:00000},{1:00},{2:HH:mm:ss.fff},{3}] {4}", 
                ProcessId, 
                Thread.CurrentThread.ManagedThreadId, 
                DateTime.UtcNow, 
                level, 
                args.Length == 0 ? format : string.Format(format, args));
        }

        private string Log(string level, Exception exc, string format, params object[] args)
        {
            var sb = new StringBuilder();
            while (exc != null)
            {
                sb.AppendLine();
                sb.AppendLine(exc.ToString());
                exc = exc.InnerException;
            }

            return string.Format(
                "[{0:00000},{1:00},{2:HH:mm:ss.fff},{3}] {4}\nEXCEPTION(S) OCCURRED:{5}", 
                ProcessId, 
                Thread.CurrentThread.ManagedThreadId, 
                DateTime.UtcNow, 
                level, 
                args.Length == 0 ? format : string.Format(format, args), 
                sb);
        }

        #endregion
    }
}