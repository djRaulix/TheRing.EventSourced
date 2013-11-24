namespace TheRing.EventSourced.Core.Log
{
    #region using

    using System;

    #endregion

    public interface ILogger
    {
        #region Public Methods and Operators

        void Debug(string text);

        void Debug(string format, params object[] args);

        void DebugException(Exception exc, string text);

        void DebugException(Exception exc, string format, params object[] args);

        void Error(string text);

        void Error(string format, params object[] args);

        void ErrorException(Exception exc, string text);

        void ErrorException(Exception exc, string format, params object[] args);

        void Fatal(string text);

        void Fatal(string format, params object[] args);

        void FatalException(Exception exc, string text);

        void FatalException(Exception exc, string format, params object[] args);

        void Info(string text);

        void Info(string format, params object[] args);

        void InfoException(Exception exc, string text);

        void InfoException(Exception exc, string format, params object[] args);

        void Warn(string text);

        void Warn(string format, params object[] args);

        void WarnException(Exception exc, string text);

        void WarnException(Exception exc, string format, params object[] args);

        void Trace(string text);

        void Trace(string format, params object[] args);

        void TraceException(Exception exc, string text);

        void TraceException(Exception exc, string format, params object[] args);

        #endregion
    }
}