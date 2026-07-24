using System;
using System.Text;

namespace STRmodsWF2SuspensionEditor.Services
{
    public static class BinaryReaderUtil
    {
        public static readonly byte[] VesuTag = Encoding.ASCII.GetBytes("usev");
        public static readonly byte[] LsevTag = Encoding.ASCII.GetBytes("lsev");

        public static bool MatchesAt(byte[] data, int offset, byte[] value)
        {
            if (data == null || value == null || offset < 0 || offset + value.Length > data.Length)
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (data[offset + i] != value[i])
                    return false;
            }

            return true;
        }

        public static int FindSequence(byte[] data, byte[] sequence, int startOffset)
        {
            if (data == null || sequence == null || sequence.Length == 0)
                return -1;

            int firstPossible = Math.Max(0, startOffset);
            int lastPossible = data.Length - sequence.Length;

            for (int i = firstPossible; i <= lastPossible; i++)
            {
                if (MatchesAt(data, i, sequence))
                    return i;
            }

            return -1;
        }

        public static int ReadInt32(byte[] data, int offset)
        {
            EnsureRange(data, offset, 4);
            return BitConverter.ToInt32(data, offset);
        }

        public static float ReadSingle(byte[] data, int offset)
        {
            EnsureRange(data, offset, 4);
            return BitConverter.ToSingle(data, offset);
        }

        public static void WriteSingle(byte[] data, int offset, float value)
        {
            EnsureRange(data, offset, 4);

            byte[] bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, data, offset, 4);
        }

        private static void EnsureRange(byte[] data, int offset, int length)
        {
            if (data == null || offset < 0 || offset + length > data.Length)
                throw new Exception("Attempted to read or write outside the file bounds.");
        }
    }
}