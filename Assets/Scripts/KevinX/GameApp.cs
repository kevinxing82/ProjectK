using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KevinX.Debugger;

namespace KevinX
{
    public class GameApp : MonoBehaviour
    {
        #region private function
        #endregion
        #region override function
        void Start()
        {
            this.gameObject.AddComponent<KXDebuggerUI>();
            KevinX.Debugger.KXDebugger.displayDebugUI = false;
        }

        void Awake()
        {

        }

        void Update()
        {
           
        }

        void LateUpdate()
        {
            
        }

        void FixedUpdate()
        {

        }

        void OnApplicationPause()
        {

        }

        void OnApplicationFocus()
        {

        }

        void OnApplicationQuit()
        {

        }

        public static  int  GetVersionNumber()
        {
            return 1;
        }
        #endregion
    }
}
