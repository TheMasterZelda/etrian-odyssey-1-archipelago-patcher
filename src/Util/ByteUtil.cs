using System.Buffers.Binary;
using System.Numerics;

namespace etrian_odyssey_ap_patcher.Util
{
    public static class ByteUtil
    {
        public static byte ReadAsByte(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return reader.ReadByte();
        }

        public static ushort ReadAsUint16(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            byte[] data = reader.ReadBytes(2);
            return (ushort)(data[1] << 0x08 | data[0]);
        }

        public static short ReadAsInt16(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            byte[] data = reader.ReadBytes(2);
            return (short)(data[1] << 0x08 | data[0]);
        }

        public static int ReadAsInt32(BinaryReader reader, int offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            byte[] data = reader.ReadBytes(4);
            return data[3] << 0x18 | data[2] << 0x10 | data[1] << 0x08 | data[0];
        }

        public static ushort ReadUint16(byte[] source, int offset)
        {
            byte[] data = new byte[2];

            for (int i = 0; i < data.Length; i++)
                data[i] = source[offset + i];

            return (ushort)(data[1] << 0x08 | data[0]);
        }

        public static short ReadInt16(byte[] source, int offset)
        {
            byte[] data = new byte[2];

            for (int i = 0; i < data.Length; i++)
                data[i] = source[offset + i];

            return (short)(data[1] << 0x08 | data[0]);
        }

        public static int ReadInt32(byte[] source, int offset)
        {
            byte[] data = new byte[4];

            for (int i = 0; i < data.Length; i++)
                data[i] = source[offset + i];

            return data[3] << 0x18 | data[2] << 0x10 | data[1] << 0x08 | data[0];
        }

        public static void Write(byte[] destination, int offset, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
                destination[offset + i] = value[i];
        }

        public static void Write(byte[] destination, int offset, ushort value)
        {
            byte[] data = GetAsLittleEndian(value);

            for (int i = 0; i < data.Length; i++)
                destination[offset + i] = data[i];
        }

        public static void Write(byte[] destination, int offset, byte value)
        {
            destination[offset] = value;
        }

        public static void Write(byte[] destination, int offset, short value)
        {
            byte[] data = GetAsLittleEndian(value);

            for (int i = 0; i < data.Length; i++)
                destination[offset + i] = data[i];
        }

        public static void Write(byte[] destination, int offset, int value)
        {
            byte[] data = GetAsLittleEndian(value);

            for (int i = 0; i < data.Length; i++)
                destination[offset + i] = data[i];
        }

        public static void Write(byte[] destination, int offset, uint value)
        {
            byte[] data = GetAsLittleEndian(value);

            for (int i = 0; i < data.Length; i++)
                destination[offset + i] = data[i];
        }

        public static void Write(MemoryStream stream, int offset, ushort value)
        {
            byte[] data = GetAsLittleEndian(value);
            stream.Seek(offset, SeekOrigin.Begin);
            stream.Write(data, 0, data.Length);
        }

        public static void Write(MemoryStream stream, int offset, short value)
        {
            byte[] data = GetAsLittleEndian(value);
            stream.Seek(offset, SeekOrigin.Begin);
            stream.Write(data, 0, data.Length);
        }

        public static void Write(MemoryStream stream, int offset, int value)
        {
            byte[] data = GetAsLittleEndian(value);
            stream.Seek(offset, SeekOrigin.Begin);
            stream.Write(data, 0, data.Length);
        }

        public static byte[] GetAsLittleEndian(ushort value)
        {
            value = BitConverter.IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value);
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetAsLittleEndian(short value)
        {
            value = BitConverter.IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value);
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetAsLittleEndian(int value)
        {
            value = BitConverter.IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value);
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetAsLittleEndian(uint value)
        {
            value = BitConverter.IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value);
            return BitConverter.GetBytes(value);
        }
    }
}
