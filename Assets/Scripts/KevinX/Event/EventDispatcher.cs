using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KevinX.Interfaces;

namespace KevinX.Event
{
    public class EventDispatcher
    {
        #region Property
        private Dictionary<int, List<IzEventListener>> _eventListenerMap = new Dictionary<int, List<IzEventListener>>();
        #endregion

        #region Public function
        public void RegisterEventListener(int eventType,IzEventListener listener)
        {
            List<IzEventListener> listenerList;
            if(this._eventListenerMap.TryGetValue(eventType,out listenerList))
            {
                if(!listenerList.Contains(listener))
                {
                    listenerList.Add(listener);
                }
            }                                               
            else
            {
                listenerList = new List<IzEventListener>();
                listenerList.Add(listener);
                this._eventListenerMap.Add(eventType, listenerList);
            }
        }

        public void RemoveEventListener(int eventType)
        {
            if (this._eventListenerMap.ContainsKey(eventType))
            {
                this._eventListenerMap.Remove(eventType);
            }
        }

        public void RemoveEventListener(int eventType,IzEventListener listener)
        {
            List<IzEventListener> listenerList;
            if(this._eventListenerMap.TryGetValue(eventType,out listenerList))
            {
                if(listenerList.Contains(listener))
                {
                    listenerList.Remove(listener);
                }
            }
        }

        public bool HasEventListener(int eventType,IzEventListener listener)
        {
            List<IzEventListener> listenerList;
            if(this._eventListenerMap.TryGetValue(eventType,out listenerList))
            {
                if(listenerList.Contains(listener))
                {
                    return true;
                }
            }
            return false;
        }

        public void DispatchEvent(int eventType,GameEvent evt)
        {
            List<IzEventListener> listenerList;
            if(this._eventListenerMap.TryGetValue(eventType,out listenerList))
            {
                for(int i=0;i<listenerList.Count;i++)
                {
                    IzEventListener listener = listenerList[i];
                    listener.ProcessEvent(evt);
                }
            }   
        }
        #endregion
    }
};
