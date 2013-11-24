namespace TheRing.EventSourced.Core.Log
{
    #region using

    using System;

    #endregion

    public class LazyLogger : ILogger
    {
        #region Fields

        private readonly Lazy<ILogger> _logger;

        #endregion

        #region Constructors and Destructors

        public LazyLogger(Func<ILogger> factory)
        {
            this._logger = new Lazy<ILogger>(factory);
        }

        #endregion

        #region Public Methods and Operators

        public void Debug(string text)
        {
            this._logger.Value.Debug(text);
        }

        public void Debug(string format, params object[] args)
        {
            this._logger.Value.Debug(format, args);
        }

        public void DebugException(Exception exc, string format)
        {
            this._logger.Value.DebugException(exc, format);
        }

        public void DebugException(Exception exc, string format, params object[] args)
        {
            this._logger.Value.DebugException(exc, format, args);
        }

        public void Error(string text)
        {
            this._logger.Value.Error(text);
        }

        public void Error(string format, params object[] args)
        {
            this._logger.Value.Error(format, args);
        }

        public void ErrorException(Exception exc, string format)
        {
            this._logger.Value.ErrorException(exc, format);
        }

        public void ErrorException(Exception exc, string format, params object[] args)
        {
            this._logger.Value.ErrorException(exc, format, args);
        }

        public void Fatal(string text)
        {
            this._logger.Value.Fatal(text);
        }

        public void Fatal(string format, params object[] args)
        {
            this._logger.Value.Fatal(format, args);
        }

        public void FatalException(Exception exc, string format)
        {
            this._logger.Value.FatalException(exc, format);
        }

        public void FatalException(Exception exc, string format, params object[] args)
        {
            this._logger.Value.FatalException(exc, format, args);
        }

        public void Info(string text)
        {
            this._logger.Value.Info(text);
        }

        public void Info(string format, params object[] args)
        {
            this._logger.Value.Info(format, args);
        }

        public void InfoException(Exception exc, string format)
        {
            this._logger.Value.InfoException(exc, format);
        }

        public void InfoException(Exception exc, string format, params object[] args)
        {
            this._logger.Value.InfoException(exc, format, args);
        }

        public void Trace(string text)
        {
            this._logger.Value.Trace(text);
        }

        public void Trace(string format, params object[] args)
        {
            this._logger.Value.Trace(format, args);
        }

        public void TraceException(Exception exc, string format)
        {
            this._logger.Value.TraceException(exc, format);
        }

        public void TraceException(Exception exc, string format, params object[] args)
        {
            this._logger.Value.TraceException(exc, format, args);
        }

        public void Warn(string text)
        {
            this._logger.Value.Warn(text);
        }

        public void Warn(string format, params object[] args)
        {
            this._logger.Value.Warn(format, args);
        }

        public void WarnException(Exception exc, string format)
        {
            this._logger.Value.WarnException(exc, format);
        }

        public void WarnException(Exception exc, string format, params object[] args)
        {
            this._logger.Value.WarnException(exc, format, args);
        }

        #endregion
    }
}