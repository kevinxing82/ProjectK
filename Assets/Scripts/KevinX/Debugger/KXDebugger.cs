using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KevinX.Debugger
{
    public class KXDebugger
    {
        private static List<string> _logContent=new List<string>();
        private static int _logCapacity = 100;

        private static bool _displayDebugUI = false;
        public static bool displayDebugUI
        {
            get
            {
                return _displayDebugUI;
            }
            set
            {
                if (_displayDebugUI != value)
                {
                    _displayDebugUI = value;
                }
            }
        }

        private static bool _all = false;
        public static bool all
        {
            get
            {
                return _all;
            }
            set
            {
                if (_all != value)
                {
                    _all = value;
                }
            }
        }


        private static bool _info = false;
        public static bool info
        {
            get
            {
                return _info;
            }
            set
            {
                if (_info != value)
                {
                    _info = value;
                }
            }
        }

        private static bool _warning = false;
        public static bool warning
        {
            get
            {
                return _warning;
            }
            set
            {
                if (_warning != value)
                {
                    _warning = value;
                }
            }
        }
        private static bool _error = false;
        public static bool error
        {
            get
            {
                return _error;
            }
            set
            {
                if(_error!=value)
                {
                    _error = value;
                }
            }
        }

        public static string content
        {
            get
            {
                string tmp="";
                int count = _logContent.Count;
                for (int i = 0; i < count; i++)
                {
                    tmp += _logContent[i] + "\n";
                }
                return tmp;
            }
        }

        private KXDebugger()
        {
        }

        private static void InitDebug()
        {
        }

        public static void Log(object message)
        {
            AddLog(message);
            Debug.Log(message);
        }
        public static void LogWarnning(object message)
        {
            AddLog(message);
            Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            AddLog(message);
            Debug.LogError(message);
        }

       

        private static void AddLog(object message)
        {
            if (_logContent == null)
            {
                KXDebugger.InitDebug();
            } 
            if(_logContent.Count>_logCapacity)
            {
                _logContent.RemoveAt(0);
            }
            _logContent.Add(message as string);
        }
    }

    public class KXDebuggerUI:MonoBehaviour
    {
        private Vector2 scrollPosition;
        void OnGUI()
        {
            if (KXDebugger.displayDebugUI)
            {
                KXDebugger.all = GUI.Toggle(new Rect(10, 10, 40, 30), KXDebugger.all, "All");
                KXDebugger.warning = GUI.Toggle(new Rect(50, 10, 80, 30), KXDebugger.warning, "Warnning");
                KXDebugger.error = GUI.Toggle(new Rect(130, 10, 100, 30), KXDebugger.error, "Error");

                scrollPosition =GUI.BeginScrollView(new Rect(0, 30, 300, 200), scrollPosition, new Rect(0, 0, 280, 1600));
                GUI.TextArea(new Rect(0, 0, 300, 1600), KXDebugger.content);
                GUI.EndScrollView();
            }
        }
    }
}
