using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KevinX.Net
{
    public class NetMsg
    {
        public ushort length = 0;
        public byte isCompress = 0;
        public byte seq = 0;
        public byte isEncry = 0;
        public ushort protocolId = 0;

        public virtual string Type()
        {
            return typeof(NetMsg).Name;
        }

        public virtual void Read(ByteArray buf)
        {
            length = (ushort)buf.ReadShort();
            protocolId = (ushort)buf.ReadShort();
        }

        public virtual void Write(ByteArray buf)
        {
            buf.WriteShort((ushort)length);
            buf.WriteByte((byte)isCompress);
            buf.WriteByte((byte)seq);
            buf.WriteByte((byte)isEncry);
            buf.WriteByte((ushort)protocolId);
        }
    }
}
