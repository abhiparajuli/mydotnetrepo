using System;
using System.IO;
using System.Text;

namespace SimpleLogLibrary
{
    public class LocalLogger
    {
        public enum LogLevel { LOG_NONE, LOG_ERROR, LOG_WARNING, LOG_DEBUG };
        private LogLevel localLogLevel;
        private String logFilePath;

        public LocalLogger()
        {
            this.localLogLevel = LogLevel.LOG_ERROR;
            this.logFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), string.Format("LocalLoggerFile{0}.log", getTotalMillisecondsUTC()));
        }

        /// <summary>
        /// Overloaded constructor that takes in log level to set and a file to write to
        /// </summary>
        /// <param name="localLogLevel"></param>
        /// <param name="logFilePath"></param>
        public LocalLogger(LogLevel localLogLevel, String logFilePath)
        {
            this.localLogLevel = localLogLevel;
            if (!String.IsNullOrEmpty(logFilePath))
            {
                this.logFilePath = logFilePath;
            }
            else
            {
                this.logFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), string.Format("LocalLoggerFile{0}.log", getTotalMillisecondsUTC()));
            }

        }

        /// <summary>
        /// helper method to generate a timestamp unique to the millisecond
        /// </summary>
        /// <returns></returns>
        private long getTotalMillisecondsUTC()
        {
            DateTime StartOfEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan mySpan = DateTime.UtcNow - StartOfEpoch;
            return (long)mySpan.TotalMilliseconds;
        }


        /// <summary>
        /// log method to be called for logging messages at a given log level
        /// </summary>
        /// <param name="msgLogLevel"></param>
        /// <param name="logText"></param>
        public void log(LogLevel msgLogLevel, String logText)
        {
            StringBuilder formattedLogText = new StringBuilder();
            try
            {
                formattedLogText.Append(DateTime.Now.ToShortDateString());
                formattedLogText.Append(" ");
                formattedLogText.Append(DateTime.Now.ToLongTimeString());

                if (msgLogLevel <= localLogLevel)
                {
                    formattedLogText.Append("   ");
                    formattedLogText.AppendLine(logText);
                    fileLogger(formattedLogText.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("[{0}       Error executing log method - {1}", formattedLogText, ex.ToString()));
                Console.WriteLine();
            }
        }

        /// <summary>
        /// helper method to help log method write to file
        /// </summary>
        /// <param name="strMessage"></param>
        private void fileLogger(String strMessage)
        {
            File.AppendAllText(logFilePath, strMessage);
        }

    }
}
