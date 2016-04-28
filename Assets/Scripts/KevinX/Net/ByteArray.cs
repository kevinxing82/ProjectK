using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KevinX.Net
{
    public class ByteArray
    {
        private int MAX_BUFF_SIZE = 65535;

        private const int SIZE_OF_BYTE = 1;
        private const int SIZE_OF_INT = 4;
        private const int SIZE_OF_LONG = 8;
        private const int SIZE_OF_SHORT = 2;
        private const int SIZE_OF_USHORT = 2;  

        protected byte[] mDataBuff;
        
        public byte[] buff
        {
            get
            {
                return mDataBuff;
            }
        }

        protected int mLength = 0;
        public int length
        {
            get
            {
                return mLength;
            }
        }

        protected int mPosition = 0;
        public int position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        public ByteArray(int bufferSize = 65535)
        {
            this.MAX_BUFF_SIZE = bufferSize;
            mDataBuff = new byte[MAX_BUFF_SIZE];
        }

        public void InitBytesArray(byte[] buff, int len)
        {
            if (len > MAX_BUFF_SIZE)
            {
                throw new Exception("InitBytesArray initialization failed!");
            }
        }

        public bool CreateFromSocketBuff(byte[] buff, int nSize)
        {
            return false;
        }

        public void WriteByte(int value)
        {
            mDataBuff[mPosition] = (byte)value;
            mPosition++;
            mLength = mPosition;
            CheckBuffSize();
        }

        public void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            EndianReverse(bytes);

            WriteData(bytes,SIZE_OF_INT);
        }

        public void WriteLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            EndianReverse(bytes);

            WriteData(bytes, SIZE_OF_LONG);
        }

        public void WriteShort(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            EndianReverse(bytes);

            WriteData(bytes,SIZE_OF_SHORT);
        }

        public void WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            short strLen = (short)bytes.Length;
            WriteShort(strLen);
            WriteData(bytes, strLen);
        }

        public void WriteBytes(byte[] bytes)
        {
            int bytesLen = bytes.Length;
            WriteInt(bytesLen);
            WriteData(bytes, bytesLen);
        }

        public int  ReadByte()
        {
            if((mPosition+1>mLength))
            {
                throw new Exception("Reading byte fail,out of limited length!");
            }
            byte value = (byte)mDataBuff[mPosition];
            mPosition++;
            return value;
        }

        public int  ReadInt()
        {
            if(mPosition+SIZE_OF_INT>mLength)
            {
                throw new Exception("ByteArray reading int fail,out of limited length!");
            }
            byte[] byteTmp = new byte[SIZE_OF_INT];
            Array.Copy(mDataBuff, mPosition, byteTmp, 0, SIZE_OF_INT);
            EndianReverse(byteTmp);
            int value = BitConverter.ToInt32(byteTmp, 0);
            mPosition += SIZE_OF_INT;
            return value;
        }

        public long ReadLong()
        {
            if (mPosition + SIZE_OF_LONG > mLength)
            {
                throw new Exception("ByteArray reading int fail,out of limited length!");
            }
            byte[] byteTmp = new byte[SIZE_OF_LONG];
            Array.Copy(mDataBuff, mPosition, byteTmp, 0, SIZE_OF_LONG);
            EndianReverse(byteTmp);
            long value = BitConverter.ToInt64(byteTmp, 0);
            mPosition += SIZE_OF_LONG;
            return value;
        }

        public short ReadShort()
        {
            if (mPosition + SIZE_OF_SHORT > mLength)
            {
                throw new Exception("ByteArray reading short fail,out of limited length!");
            }
            byte[] byteTmp = new byte[SIZE_OF_SHORT];
            Array.Copy(mDataBuff, mPosition, byteTmp, 0, SIZE_OF_SHORT);
            EndianReverse(byteTmp);
            short value = BitConverter.ToInt16(byteTmp, 0);
            mPosition += SIZE_OF_SHORT;
            return value;
        }

        public ushort ReadUShort()
        {
            if (mPosition + SIZE_OF_USHORT > mLength)
            {
                throw new Exception("ByteArray reading short fail,out of limited length!");
            }
            byte[] byteTmp = new byte[SIZE_OF_USHORT];
            Array.Copy(mDataBuff, mPosition, byteTmp, 0, SIZE_OF_USHORT);
            EndianReverse(byteTmp);
            ushort value = BitConverter.ToUInt16(byteTmp, 0);
            mPosition += SIZE_OF_USHORT;
            return value;
        }

        public string ReadString()
        {
            if (mPosition + SIZE_OF_SHORT > mLength)
            {
                throw new Exception("ByteArray reading short fail,out of limited length!");
            }
            byte[] byteTmp = new byte[SIZE_OF_SHORT];
            Array.Copy(mDataBuff, mPosition, byteTmp, 0, SIZE_OF_SHORT);
            EndianReverse(byteTmp);
            short strLen = BitConverter.ToInt16(byteTmp, 0);
            mPosition += SIZE_OF_SHORT;

            string value = Encoding.UTF8.GetString(mDataBuff, mPosition, strLen);
            mPosition += strLen;
            return value;
        }

        public byte[] ReadBytes()
        {
            if (mPosition + SIZE_OF_INT > mLength)
            {
                throw new Exception("ByteArray reading short fail,out of limited length!");
            }
            byte[] byteTmp = new byte[SIZE_OF_INT];
            Array.Copy(mDataBuff, mPosition, byteTmp, 0, SIZE_OF_INT);
            EndianReverse(byteTmp);
            int bytesLength = BitConverter.ToInt32(byteTmp, 0);
            mPosition += SIZE_OF_INT;

            byte[] byteBuff = new byte[bytesLength];
            Array.Copy(mDataBuff, mPosition, byteBuff, 0, bytesLength);
            mPosition += bytesLength;
            return byteBuff;
        }

        public void EndianReverse(Array array)
        {
            // 不同的计算机结构采用不同的字节顺序存储数据。
            //“Big - endian”表示最大的有效字节位于单词的左端。
            //“Little - endian”表示最大的有效字节位于单词的右端。
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
        }

        public bool CopyFromByteArray(ref byte[] aBuff,ref int nSize)
        {
            if(aBuff==null)
            {
                return false;
            }
            Array.Copy(this.mDataBuff, 0, aBuff, 0,this.mLength);
            nSize = mLength;
            return true;
        }

        private void WriteData(byte[] bytes,int size)
        {
            Array.Copy(bytes, 0, mDataBuff, mPosition, size);
            mPosition += size;
            mLength = mPosition;
            CheckBuffSize();
        }

        private void CheckBuffSize()
        {
            if(mPosition>MAX_BUFF_SIZE)
            {
                throw new Exception("Out the limited of bytearray size!");
            }
        }
    }
}
