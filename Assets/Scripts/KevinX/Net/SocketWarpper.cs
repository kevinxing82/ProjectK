using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using KevinX.Debugger;

namespace KevinX.Net
{
    public class SocketWarpper
    {
        #region Property
        private byte[] _recvBuff = new byte[1048560];
        private Socket _socket;
        private Queue<ByteArray> _recvQueue = new Queue<ByteArray>();
        private int _recvPosition = 0;
        private byte id = 0;

        public Action<bool> connectedCallback;

        public int MessageCount
        {
            get
            {
                return _recvQueue.Count;
            }
        }
        #endregion

        #region Public function
        public bool SyncConnect(string ip,int port)
        {
            if(_socket!=null)
            {
                Disconnect();
            }
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if(_socket==null)
            {
                connectedCallback(false);
                return false;
            }
            try
            {
                _socket.Connect(ip, port);
            }
            catch(Exception exception)
            {
                KXLogger.LogError(exception.Message + " Exception Error: " + exception.StackTrace);
                connectedCallback(false);
                return false;
            }
            _recvPosition = 0;
            if(!AsyncRecvMessageFromSocket())
            {
                Disconnect();
                connectedCallback(false);
                return false;
            }
            connectedCallback(true);
            return true;
        }

        public void AsyncConnect(string ip, int port)
        {
            if(_socket!=null)
            {
                Disconnect();
            }
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if(_socket==null)
            {
                connectedCallback(false);
                return;
            }
            try
            {
                _socket.BeginConnect(ip,port, new AsyncCallback(AsyncConnectCallback), this);
            }
            catch(Exception exception)
            {
                KXLogger.LogError(exception.Message + " Exception error : " + exception.StackTrace);
                connectedCallback(false);
                return;
            }
        }

        public void Disconnect()
        {
            if(IsConnected())
            {
                try
                {
                    id = 0;
                    _socket.Disconnect(true);
                }
                catch(Exception ex)
                {
                    KXLogger.LogError("Disconnect error: " + ex.StackTrace);
                }
                _socket = null;
                _recvPosition = 0;
            }
        }

        public bool IsConnected()
        {
            return _socket!=null&&_socket.Connected;
        }

        public bool Send(NetMsg msg)
        {
             if(!this.IsConnected()||msg==null)
            {
                return false;
            }
            ByteArray body = new ByteArray(2097120);
            msg.seq = GetMessageId();
            msg.Write(body);
            ushort len = (ushort)body.length;
            Byte[] lenBytes = BitConverter.GetBytes((ushort)(len - 2));
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(lenBytes);
            }
            Array.Copy(lenBytes, body.buff, 2);

            try
            {
                SocketError err;
                _socket.BeginSend(body.buff, 0, body.length, SocketFlags.None, out err, new AsyncCallback(AsyncSendCallback), this);
            }
            catch(Exception ex)
            {
                KXLogger.LogError("Send socket data error : " + ex.StackTrace);
                return false;
            }
            return true;
        }

        public ByteArray RecMessage()
        {
            if(_socket==null)
            {
                return null;
            }
            if(_recvQueue.Count==0)
            {
                return null;
            }
            ByteArray msg = _recvQueue.Dequeue();
            return msg;
        }

        public void Reset()
        {
            _recvQueue.Clear();
        }
        #endregion

        #region Private function
        private void AsyncConnectCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndConnect(ar);
                _recvPosition = 0;
                if(!AsyncRecvMessageFromSocket())
                {
                    Disconnect();
                    connectedCallback(false);
                }
                connectedCallback(true);
            }
            catch(Exception ex)
            {
                KXLogger.LogError(ex.Message + " connect server callback error: " + ex.StackTrace);
                connectedCallback(false);
            }
        }

        private bool AsyncRecvMessageFromSocket()
        {
            if(_socket!=null)
            {
                try
                {
                    SocketError socketError;
                    _socket.BeginReceive(
                        _recvBuff,
                        _recvPosition,
                        _recvBuff.Length - _recvPosition,
                        SocketFlags.None,
                        out socketError,
                        new AsyncCallback(AsyncRecvCallback), this);
                }
                catch(Exception exception)
                {
                    KXLogger.LogError("Receive socket error: " + exception.StackTrace);
                    return false;
                }
                return true;
            }
            return false;
        }

        private void AsyncRecvCallback(IAsyncResult ar)
        {
            int num = 0;
            try
            {
                SocketError se;
                num = _socket.EndReceive(ar, out se);
                if(num<=0)
                {
                    KXLogger.LogError("Receive data length : " + num.ToString());
                }
            }
            catch(Exception ex)
            {
                 if(IsConnected())
                {
                    KXLogger.LogError(ex.Message + "SocketWrapper.AsyncRecvCallback Error : "+ex.StackTrace);
                }
                return;
            }

            if(num==0)
            {
                KXLogger.LogError("Receive socket data length = 0 : Disconnect()");
                Disconnect();
            }
            else
            {
                _recvPosition += num;
                while(true)
                {
                    if(!this.IsConnected())
                    {
                        break;
                    }
                    ByteArray byteArray = new ByteArray(1048560);
                    if(!this.ParseMessage(ref _recvBuff,ref _recvPosition,ref byteArray))
                    {
                        break;
                    }
                    _recvQueue.Enqueue(byteArray);
                }
                AsyncRecvMessageFromSocket();
            }
        }

        private bool ParseMessage(ref byte[] buf,ref int len,ref ByteArray byteArray)
        {
            if(len<4)
            {
                return false;
            }

            byte[] byteTmp = new byte[2];
            Array.Copy(buf, 0, byteTmp, 0, 2);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteTmp);
            }
            int size = BitConverter.ToUInt16(byteTmp, 0) + 2;

            if(size<3||size>65535)
            {
                this.Disconnect();
                KXLogger.LogError("Data package too long or too short,:size = "+size.ToString());
                return false;
            }
            if(len<size)
            {
                return false;
            }
            if(!byteArray.CreateFromSocketBuff(buf,size))
            {
                RemoveRecvBuff(ref buf, ref len, size);
                return false;
            }
            RemoveRecvBuff(ref buf, ref len, size);
            return true;
        }

        private void RemoveRecvBuff(ref byte[] buff,ref int len,int size)
        {
            if(size<=len)
            {
                byte[] destinationArray = new byte[65535];
                Array.Copy(buff, size, destinationArray, 0, len - size);
                Array.Copy(destinationArray, 0, buff, 0, len - size);
                len -= size;
            }
        }

        private void AsyncSendCallback(IAsyncResult ar)
        {
            try
            {
                SocketError err;
                _socket.EndSend(ar, out err);
            }
            catch(Exception ex)
            {
                KXLogger.LogError("Sending socket data callback error : " + ex.StackTrace);
            }
        }

        private byte GetMessageId()
        {
            if(id>=127)
            {
                id = 0;
            }
            return id++;
        }
        #endregion
    }
}
