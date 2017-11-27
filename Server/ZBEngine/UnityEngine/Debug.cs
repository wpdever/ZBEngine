using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ZBEngine
{
    public static class Debug
    {
        public static event LogCallback logMessageReceived = ConsoleLog;

        public delegate void LogCallback(string condition, string stackTrace, LogType type, int userID, string cmd);

        public static bool IsShowToConsole = true;

        static Debug()
        {
            
        }

        private static void ConsoleLog(string condition, string stackTrace, LogType type, int userID, string cmd)
        {
            //由于这个流程会比较卡，所以全部做成了异步线程的方式
            AsyncThread.Post(() => {
                if (IsShowToConsole)
                {
                    switch (type)
                    {
                        case LogType.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                        case LogType.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                        case LogType.Exception: Console.ForegroundColor = ConsoleColor.Magenta; break;
                        case LogType.Assert: Console.ForegroundColor = ConsoleColor.Gray; break;
                        case LogType.Log: Console.ForegroundColor = ConsoleColor.White; break;
                    }
                    if (type == LogType.Exception)
                    {
                        Console.WriteLine();
                        if (cmd == null)
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "]: " + condition);
                        }
                        else
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "][userID:" + userID
                            + "][" + cmd + "]: " + condition);
                        }
                        Console.WriteLine(stackTrace);
                    }
                    else
                    {
                        if (cmd == null)
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "]: " + condition);
                        }
                        else
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "][userID:" + userID
                            + "][" + cmd + "]: " + condition);
                        }
                    }
                    Console.ResetColor();
                }
            });
        }

        public static void Log(object message, int userID = 0, string cmd = null)
        {
            logMessageReceived.Invoke(message == null ? "NULL" : message.ToString(), null, LogType.Log, userID, cmd);
        }

        public static void LogWarning(object message, int userID = 0, string cmd = null)
        {
            logMessageReceived.Invoke(message == null ? "NULL" : message.ToString(), null, LogType.Warning, userID, cmd);
        }

        public static void LogError(object message, int userID = 0, string cmd = null)
        {
            logMessageReceived.Invoke(message == null ? "NULL" : message.ToString(), null, LogType.Error, userID, cmd);
        }

        public static void LogException(object message, int userID = 0, string cmd = null)
        {
            logMessageReceived.Invoke(message == null ? "NULL" : message.ToString(), stackTrace(), LogType.Exception, userID, cmd);
        }

        public static void LogAssertion(object message, int userID = 0, string cmd = null)
        {
            logMessageReceived.Invoke(message == null ? "NULL" : message.ToString(), null, LogType.Assert, userID, cmd);
        }

        private static string stackTrace()
        {
            StackTrace st = new StackTrace(true);
            StringBuilder result = new StringBuilder();
            StackFrame[] sfs = st.GetFrames();
            for (int u = 2; u < sfs.Length; ++u)
            {
                string fileName = sfs[u].GetFileName();
                if (string.IsNullOrEmpty(fileName))
                {
                    result.AppendFormat("[STACK][{0}]: {1}\n", u - 2, sfs[u].GetMethod().ToString());
                }
                else
                {
                    result.AppendFormat("[STACK][{0}]: {1}\n        at: {2} [{3},{4}]\n", u - 2, sfs[u].GetMethod().ToString(), fileName, sfs[u].GetFileLineNumber(), sfs[u].GetFileColumnNumber());
                }
            }
            return result.ToString();
        }
    }
}
