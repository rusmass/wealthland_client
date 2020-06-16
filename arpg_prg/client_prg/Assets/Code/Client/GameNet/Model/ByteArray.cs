using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
/// <summary>
/// Byte array.处理byte数组
/// </summary>
public class ByteArray
{
    private List<byte> bytes = new List<byte>();

    public ByteArray()
    {       
    }

	/// <summary>
	/// Initializes a new instance of the <see cref="ByteArray"/> class. 初始化
	/// </summary>
	/// <param name="buffer">Buffer.</param>
    public ByteArray(byte[] buffer)
    {
        for (int i = 0; i < buffer.Length; i++) {
            bytes.Add(buffer[i]);
        }
    }

	/// <summary>
	/// Gets the length.返回长度
	/// </summary>
	/// <value>The length.</value>
    public int Length
    {
        get {return bytes.Count;}
    }

    public int Postion
    {
        get ; 
        set ; 
    }

	/// <summary>
	/// Gets the buffer. 返回byte 数组
	/// </summary>
	/// <value>The buffer.</value>
    public byte[] Buffer
    {
        get { return bytes.ToArray(); }
    }

	/// <summary>
	/// Reads the boolean. 判断是否读取完
	/// </summary>
	/// <returns><c>true</c>, if boolean was  read, <c>false</c> otherwise.</returns>
    public bool ReadBoolean()
    {
        byte b = bytes[Postion];
        Postion += 1;
        return b==(byte)0?false:true;
    }

	//读取byte
    public byte ReadByte()
    {
        byte result = bytes[Postion];
        Postion += 1;
        return result;
    }

	/// <summary>
	/// Writes the int. 写入、插入int
	/// </summary>
	/// <param name="value">Value.</param>
    public void WriteInt(int value) { 
        byte[] bs= new byte[4];
	    bs[0] = (byte)(value >> 24);
	    bs[1] = (byte)(value >> 16);
	    bs[2] = (byte)(value >> 8);
	    bs[3] = (byte)(value);
        bytes.AddRange(bs);
    }

	/// <summary>
	/// Reads the int. 读取int
	/// </summary>
	/// <returns>The int.</returns>
    public int ReadInt()
    {
        byte[] bs=new byte[4];
        for (int i = 0; i < 4; i++) {
            bs[i] = bytes[i + Postion];
        }
        Postion += 4;
	    int result = (int)bs[3] | ((int)bs[2] << 8) | ((int)bs[1] << 16) | ((int)bs[0] << 24);
        return result;
    }

	/// <summary>
	/// Reads the UTF bytes. 读取string
	/// </summary>
	/// <returns>The UTF bytes.</returns>
	/// <param name="length">Length.</param>
    public string ReadUTFBytes(uint length)
    {
        if (length == 0)
            return string.Empty;
        byte[] b = new byte[length];
        for (int i = 0; i < length; i++) {
            b[i] = bytes[i + Postion];
        }
        Postion += (int)length;
        string decodedString = Encoding.UTF8.GetString(b);
        return decodedString;
    }

	/// <summary>
	/// Writes the UTF bytes.写入string
	/// </summary>
	/// <param name="value">Value.</param>
    public void WriteUTFBytes(string value)
    {
        byte[] bs = Encoding.UTF8.GetBytes(value);
        bytes.AddRange(bs);        
    }

	/// <summary>
	/// Writes the boolean. 写入bool类型
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
    public void WriteBoolean(bool value)
    {
        bytes.Add(value ? ((byte)1) : ((byte)0));
    }

	/// <summary>
	/// Writes the byte. 写入单个字节
	/// </summary>
	/// <param name="value">Value.</param>
    public void WriteByte(byte value)
    {
        bytes.Add(value);
    }

	/// <summary>
	/// Writes the bytes. 
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="offset">Offset.</param>
	/// <param name="length">Length.</param>
    public void WriteBytes(byte[] value,int offset,int length)
    {
        for (int i = 0; i < length; i++) {
            bytes.Add(value[i + offset]);
        }
    }
	/// <summary>
	/// Writes the bytes. 写入 byte 数组
	/// </summary>
	/// <param name="value">Value.</param>
    public void WriteBytes(byte[] value)
    {
        bytes.AddRange(value);
    }
 

}
