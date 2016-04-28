using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KevinX.Event
{
    public enum GameEventType
    {
        SOCKET_CONNECTED = 0,
        SOCKET_CONNECTED_FAIL,
        SOCKET_DISCONNECTED,
        SOCKET_MESSAGE_SEND,
        SOCKET_MESSAGE_RECEIVE,
}
    public class EventConst
    {
    }
}
