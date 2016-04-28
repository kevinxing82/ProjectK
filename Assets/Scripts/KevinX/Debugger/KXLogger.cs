using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KevinX.Debugger
{
    public class KXLogger
    {
        public static LoggerLevel level = LoggerLevel.All;

        public static void Log(string msg)
        {
            if (level > LoggerLevel.None)
            {
                Debug.Log(msg);
            }
        }

        public static void LogFormat(string content, params object[] args)
        {
            if (level > LoggerLevel.None)
            {
                Debug.Log(string.Format(content, args));
            }
        }

        public static void LogWarning(string msg)
        {
            if (level > LoggerLevel.Warn)
            {
                Debug.LogWarning(msg);
            }
        }

        public static void LogWarningFormat(string content, params object[] args)
        {
            if (level > LoggerLevel.Warn)
            {
                Debug.LogWarningFormat(string.Format(content, args));
            }
        }

        public static void LogError(string msg)
        {
            if (level > LoggerLevel.Error)
            {
                Debug.LogError(msg);
            }
        }

        public static void LogError(string msg, Exception ex)
        {
            if (level > LoggerLevel.Error)
            {
                LogError(msg + "\n Error Type:" + ex.Message + "\n Detail Info: \n" + ex.StackTrace);
            }
        }

        public static void LogErrorFormat(string content, params object[] args)
        {
            if(level>LoggerLevel.Error)
            {
                LogError(string.Format(content, args));
            }
        }
    }

    public enum LoggerLevel
    {
        None=0,
        Warn=1,
        Error=2,
        All=3,
    }
}
