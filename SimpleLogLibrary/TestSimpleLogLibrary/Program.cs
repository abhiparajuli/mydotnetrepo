using SimpleLogLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSimpleLogLibrary
{
    class Program
    {
        /// <summary>
        /// test harness for the simple log library
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LocalLogger.LogLevel setLevel = LocalLogger.LogLevel.LOG_DEBUG;
            if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "0":
                        setLevel = LocalLogger.LogLevel.LOG_NONE;
                        break;
                    case "1":
                        setLevel = LocalLogger.LogLevel.LOG_ERROR;
                        break;
                    case "2":
                        setLevel = LocalLogger.LogLevel.LOG_WARNING;
                        break;
                    case "3":
                        setLevel = LocalLogger.LogLevel.LOG_DEBUG;
                        break;
                }
            }
            LocalLogger myLogger = new LocalLogger(setLevel, null);
            testMessages(myLogger);
        }

        /// <summary>
        /// helper method for the log library test harness
        /// </summary>
        /// <param name="myLogger"></param>
        private static void testMessages(LocalLogger myLogger)
        {
            myLogger.log(LocalLogger.LogLevel.LOG_DEBUG, "My test debug message.");
            myLogger.log(LocalLogger.LogLevel.LOG_WARNING, "My test warning message.");
            myLogger.log(LocalLogger.LogLevel.LOG_ERROR, "My test error message.");
            myLogger.log(LocalLogger.LogLevel.LOG_NONE, "My test none message.");
        }
    }
}
