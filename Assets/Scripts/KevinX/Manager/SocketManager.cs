#define TRACE_MSG_SEND_REV_RESULT
#define IGNORE_TRACE_SOME_MSG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KevinX.Event;
using KevinX.Interfaces;
using KevinX.Net;
using KevinX.Debugger;

namespace KevinX.Manager
{
    public class SocketManager:EventDispatcher,IzTick
    {
        public bool ConnectSign = false;

        private SocketWarpper _socket;
        private Dictionary<string, Type> _types = new Dictionary<string, Type>();
        private int _handleCount;

#if IGNORE_TRACE_SOME_MSG
        private List<string> _listIgnoreTraceMsg;
#endif

        #region Singleton
        private static SocketManager _ins;
        public  static SocketManager instance
        {
            get
            {
                if(_ins==null)
                {
                    _ins = new SocketManager();
                }
                return _ins;
            }
        }
        #endregion
        #region      Constructor
        private SocketManager()
        {

        }
        #endregion

        #region      Public function 
        public void AsyncConnectSocket(string ip,int port)
        {
            _socket.connectedCallback = ConnectCallback;
            _socket.AsyncConnect(ip, port);
        }   
        
        public void SyncConnectSocket(string ip,int port)
        {
            _socket.connectedCallback = ConnectCallback;
            _socket.SyncConnect(ip, port);
        }    

        public void Disconnect()
        {
            _socket.Disconnect();
        }

        public void SendMessage(NetMsg msg)
        {
            bool bRes = _socket.Send(msg);
#if TRACE_MSG_SEND_REV_RESULT
            if (!bRes)
            {
#if IGNORE_TRACE_SOME_MSG
                if (_listIgnoreTraceMsg.IndexOf(msg.Type()) == -1)
                {
                    KXLogger.Log(msg.Type() + " sending failed!");
                }
#else
                KXLogger.Log(msg.Type() + " sending failed!");
#endif
            }
            else
            {
#if IGNORE_TRACE_SOME_MSG
                if (_listIgnoreTraceMsg.IndexOf(msg.Type()) == -1)
                {
                    KXLogger.Log(msg.Type() + " sent successfully!");
                }
#else
                KXLogger.Log(msg.Type() + " sent successfully!");
#endif
                DispatchEvent((int)GameEventType.SOCKET_MESSAGE_SEND, new GameEvent(this, (int)GameEventType.SOCKET_MESSAGE_SEND));
            }
#endif
        }

        public bool IsConnected()
        {
            return _socket.IsConnected();
        }
        public void OnTick(float dt)
        {
            if(!IsConnected()&&ConnectSign)
            {
                ConnectSign = false;
                DispatchEvent((int)GameEventType.SOCKET_DISCONNECTED, new GameEvent(this,(int)GameEventType.SOCKET_DISCONNECTED));

            }
            if(dt>=0.05)
            {
                _handleCount = _socket.MessageCount / 2;
                _handleCount = Mathf.Clamp(_handleCount, 1, _socket.MessageCount);
                while(_handleCount>0)
                {
                    DecodeByteArray(_socket.RecMessage());
                    _handleCount--;
                }
            }
            else
            {
                DecodeByteArray(_socket.RecMessage());
            }
        }
        #endregion

        #region Private function
        private void ConnectCallback(bool succ)
        {

        }

        private void DecodeByteArray(ByteArray byteArray)
        {
             if(byteArray!=null)
            {

            }
        }
        #endregion
    }

}
