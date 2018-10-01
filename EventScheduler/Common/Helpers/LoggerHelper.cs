using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class LoggerHelper
    {
        #region Instance
        private static LoggerHelper _instance;
        private static readonly object syncLock = new object();
        private LoggerHelper()
        {
            ClientLogBuilder = new StringBuilder();
            ServerLogBuilder = new StringBuilder();
        }
        public static LoggerHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoggerHelper();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public StringBuilder ClientLogBuilder { get; set; }
        public StringBuilder ServerLogBuilder { get; set; }

        public String LogMessage(String message, EEventPriority eventPriority, EStringBuilder stringBuilderType)
        {
            StringBuilder stringBuilder = GetStringBuilder(stringBuilderType);
            stringBuilder.Append($"{GetStringType(eventPriority)} | {DateTime.Now} | {message} {Environment.NewLine}");
            return stringBuilder.ToString();
        }

        private String GetStringType(EEventPriority eventPriority)
        {
            String stringType = "";

            switch (eventPriority)
            {
                case EEventPriority.DEBUG:
                    stringType = "DEBUG";
                    break;
                case EEventPriority.INFO:
                    stringType = "INFO";
                    break;
                case EEventPriority.WARN:
                    stringType = "WARN";
                    break;
                case EEventPriority.ERROR:
                    stringType = "ERROR";
                    break;
                case EEventPriority.FATAL:
                    stringType = "FATAL";
                    break;
                default:
                    break;
            }

            return stringType;
        }
        private StringBuilder GetStringBuilder(EStringBuilder stringBuilderType)
        {
            if (stringBuilderType == EStringBuilder.CLIENT)
            {
                return ClientLogBuilder;
            }
            else
            {
                return ServerLogBuilder;
            }
        }
    }
}
