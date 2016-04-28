using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KevinX.Debugger
{
    public class DebuggerUI : MonoBehaviour
    {
        private Vector2 scrollPosition;

        private static List<string> _logContent = new List<string>();
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

        public static string content
        {
            get
            {
                string tmp = "";
                int count = _logContent.Count;
                for (int i = 0; i < count; i++)
                {
                    tmp += _logContent[i] + "\n";
                }
                return tmp;
            }
        }

        void OnGUI()
        {
            if (displayDebugUI)
            {
                KXDebugger.all = GUI.Toggle(new Rect(10, 10, 40, 30), KXDebugger.all, "All");
                KXDebugger.warning = GUI.Toggle(new Rect(50, 10, 80, 30), KXDebugger.warning, "Warnning");
                KXDebugger.error = GUI.Toggle(new Rect(130, 10, 100, 30), KXDebugger.error, "Error");

                scrollPosition = GUI.BeginScrollView(new Rect(0, 30, 300, 200), scrollPosition, new Rect(0, 0, 280, 1600));
                GUI.TextArea(new Rect(0, 0, 300, 1600), KXDebugger.content);
                GUI.EndScrollView();
            }
        }
    }
}
