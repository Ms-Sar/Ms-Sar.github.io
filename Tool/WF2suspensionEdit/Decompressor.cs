using System;
using System.Collections.Generic;

namespace STRmodsWF2SuspensionEditor
{
    public static class Decompressor
    {
        public static byte[] Decompress(byte[] data)
        {
            if (data == null || data.Length < 12)
                throw new Exception("File is too small to be a valid bag file.");

            byte[] processedData = data;
            byte firstByte = processedData[0];

            if (firstByte == 0x01)
                return processedData;

            if (firstByte == 0x08)
                throw new Exception("Encrypted data (type 08) is not supported.");

            int blockOffset = 1;

            if (firstByte == 0x07 || firstByte == 0x0A)
                blockOffset = 2;
            else if (firstByte != 0x04)
                throw new Exception("Invalid or unsupported compressed file format.");

            var output = new List<byte>();

            for (int i = 0; i < 12; i++)
                output.Add(processedData[i]);

            output[0] = 0x01;

            int filePos = 12;

            while (filePos < processedData.Length)
            {
                if (filePos + (blockOffset * 4) > processedData.Length)
                    throw new Exception("Unexpected end of file while reading compressed block header.");

                int blockSize = BitConverter.ToInt32(processedData, filePos);
                filePos += blockOffset * 4;

                if (blockSize < 0 || filePos + blockSize > processedData.Length)
                    throw new Exception("Compressed block size is invalid.");

                int blockEnd = filePos + blockSize;

                while (filePos < blockEnd)
                {
                    byte token = processedData[filePos++];
                    int literalLength = (token & 0xF0) >> 4;
                    int matchLength = (token & 0x0F) + 4;

                    if (literalLength == 0x0F)
                    {
                        byte b;
                        do
                        {
                            if (filePos >= blockEnd)
                                throw new Exception("Invalid extended literal length.");

                            b = processedData[filePos++];
                            literalLength += b;
                        }
                        while (b == 0xFF);
                    }

                    if (filePos + literalLength > blockEnd)
                        throw new Exception("Literal data exceeds compressed block bounds.");

                    for (int i = 0; i < literalLength; i++)
                        output.Add(processedData[filePos++]);

                    if (filePos >= blockEnd)
                        break;

                    if (filePos + 2 > blockEnd)
                        throw new Exception("Missing LZ4 match offset.");

                    ushort offset = BitConverter.ToUInt16(processedData, filePos);
                    filePos += 2;

                    if (offset == 0 || offset > output.Count)
                        throw new Exception("Invalid LZ4 match offset.");

                    if (matchLength == 19)
                    {
                        byte b;
                        do
                        {
                            if (filePos >= blockEnd)
                                throw new Exception("Invalid extended match length.");

                            b = processedData[filePos++];
                            matchLength += b;
                        }
                        while (b == 0xFF);
                    }

                    int matchPos = output.Count - offset;

                    for (int i = 0; i < matchLength; i++)
                        output.Add(output[matchPos + i]);
                }

                if (filePos != blockEnd)
                    throw new Exception("Compressed block did not end at the expected offset.");
            }

            return output.ToArray();
        }
    }
}