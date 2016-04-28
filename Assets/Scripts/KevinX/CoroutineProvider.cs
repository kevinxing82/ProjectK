using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KevinX
{
    public class CoroutineProvider:MonoBehaviour
    {
        void Awake()
        {

        }

        void Start()
        {
        }

        private static CoroutineProvider _ins = null;
        public static CoroutineProvider instance
        {
            get
            {
                if(_ins==null)
                {
                    _ins = GameObject.Find("GlobalScriptObject").AddComponent<CoroutineProvider>();
                }
                return _ins;
            }
        }
    }
}
