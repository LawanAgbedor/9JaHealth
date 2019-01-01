using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UTIL
{
    public class LoggerData : Dictionary<string, object>
    {
        public LoggerData() : base() { }
        public LoggerData(IDictionary<string, object> data) : base(data) { }

        public override string ToString()
        {
            var _data = "(empty)";
            if (this.Any())
            {
                _data = string.Join("|", this.Select(a => $"{a.Key}={a.Value}"));
            }
            return _data;
        }
    }

    public class Logger<T> where T : class
    {
        public static readonly string LOG_DATA = "logger_data";
        public static readonly string LOG_ENVIRONMENT = "logger_environment";

        private ILog logger;

        public Logger()
        {
            logger = LogManager.GetLogger(typeof(T));
            SetGlobalProperties();
        }

        private void SetGlobalProperties()
        {
            GlobalContext.Properties[LOG_ENVIRONMENT] = ConfigHelper.GetValue("ENVIRONMENT");
        }

        private LoggerData UpdateLogData(LoggerData logData, string memberName, string sourceFilePath, int callerLineNumber)
        {
            addMemberName(logData, memberName);
            addSourceFileName(logData, sourceFilePath);
            addCallerLineNumber(logData, callerLineNumber);
            return logData;
        }

        private LoggerData addSourceFileName(LoggerData logData, string sourceFile)
        {
            return addKeyValue(logData, "SourceFile", sourceFile);
        }

        private LoggerData addMemberName(LoggerData logData, string memberName)
        {
            return addKeyValue(logData, "MemberName", memberName);
        }

        private LoggerData addCallerLineNumber(LoggerData logData, int callerLineNumber)
        {
            return addKeyValue(logData, "CallerLineNumber", callerLineNumber);
        }

        private LoggerData addKeyValue(LoggerData logData, string key, object value)
        {
            logData = logData ?? new LoggerData();
            logData.Add(key, value);
            return logData;
        }

        private void BuildLoggerData(LoggerData data)
        {
            var _data = data == null ? "(null)" : data.ToString();                
            LogicalThreadContext.Properties[LOG_DATA] = _data;
        }

        public void Debug(object message, LoggerData logData = null, Exception exception = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            logData = UpdateLogData(logData, memberName, sourceFilePath, callerLineNumber);
            BuildLoggerData(logData);
            _Debug(message, exception);
        }
        public void Error(object message, LoggerData logData = null, Exception exception = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            logData = UpdateLogData(logData, memberName, sourceFilePath, callerLineNumber);
            BuildLoggerData(logData);
            _Error(message, exception);
        }
        public void Fatal(object message, LoggerData logData = null, Exception exception = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            logData = UpdateLogData(logData, memberName, sourceFilePath, callerLineNumber);
            BuildLoggerData(logData);
            _Fatal(message, exception);
        }
        public void Info(object message, LoggerData logData = null, Exception exception = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            logData = UpdateLogData(logData, memberName, sourceFilePath, callerLineNumber);
            BuildLoggerData(logData);
            _Info(message, exception);
        }
        public void Warning(object message, LoggerData logData = null, Exception exception = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            logData = UpdateLogData(logData, memberName, sourceFilePath, callerLineNumber);
            BuildLoggerData(logData);
            _Warning(message, exception);
        }


        private void _Debug(object message, Exception exception = null)
        {
            logger.Debug(message, exception);
        }
        private void _Error(object message, Exception exception = null)
        {
            logger.Error(message, exception);
        }
        private void _Fatal(object message, Exception exception = null)
        {
            logger.Fatal(message, exception);
        }
        private void _Info(object message, Exception exception = null)
        {
            logger.Info(message, exception);
        }
        private void _Warning(object message, Exception exception = null)
        {
            logger.Warn(message, exception);
        }
    }
}
