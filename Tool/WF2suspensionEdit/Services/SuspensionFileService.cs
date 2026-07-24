using System;
using System.Collections.Generic;
using System.IO;
using STRmodsWF2SuspensionEditor.Models;

namespace STRmodsWF2SuspensionEditor.Services
{
    public sealed class SuspensionFileService
    {
        private const int Wf1VesuVersion = 10;
        private const int Wf2VesuVersion = 14;

        private const int Wf1LsevVersion = 8;
        private const int Wf2LsevVersion = 10;

        private byte[] _wf2Data;
        private int _frontFloatOffset;
        private int _rearFloatOffset;

        public string CurrentWf2Path { get; private set; }

        public bool HasWf2File
        {
            get { return _wf2Data != null; }
        }

        public SuspensionData LoadWf2Upgrade(string path)
        {
            byte[] compressedData = File.ReadAllBytes(path);
            byte[] decompressedData = Decompressor.Decompress(compressedData);

            int vesuOffset = FindVesu(
                decompressedData,
                Wf2VesuVersion);

            int[] suspensionOffsets = FindLsevFloatOffsets(
                decompressedData,
                vesuOffset + 8,
                Wf2LsevVersion,
                SuspensionMapper.Wf2FloatCountPerAxle);

            _wf2Data = decompressedData;
            _frontFloatOffset = suspensionOffsets[0];
            _rearFloatOffset = suspensionOffsets[1];
            CurrentWf2Path = path;

            return ReadWf2SuspensionData(
                _wf2Data,
                _frontFloatOffset,
                _rearFloatOffset);
        }

        public SuspensionData LoadWf1Vesu(string path)
        {
            byte[] compressedData = File.ReadAllBytes(path);
            byte[] decompressedData = Decompressor.Decompress(compressedData);

            int vesuOffset = FindVesu(
                decompressedData,
                Wf1VesuVersion);

            int[] suspensionOffsets = FindLsevFloatOffsets(
                decompressedData,
                vesuOffset + 8,
                Wf1LsevVersion,
                SuspensionMapper.Wf1FloatCountPerAxle);

            float[] frontWf1Values = ReadRawFloatBlock(
                decompressedData,
                suspensionOffsets[0],
                SuspensionMapper.Wf1FloatCountPerAxle);

            float[] rearWf1Values = ReadRawFloatBlock(
                decompressedData,
                suspensionOffsets[1],
                SuspensionMapper.Wf1FloatCountPerAxle);

            float[] frontWf2Layout =
                SuspensionMapper.ConvertWf1ValuesToWf2Values(
                    frontWf1Values);

            float[] rearWf2Layout =
                SuspensionMapper.ConvertWf1ValuesToWf2Values(
                    rearWf1Values);

            return new SuspensionData(
                frontWf2Layout,
                rearWf2Layout);
        }

        public void SaveWf2Upgrade(
            string outputPath,
            SuspensionData data)
        {
            if (_wf2Data == null)
            {
                throw new Exception(
                    "No Wreckfest 2 .upgr file has been loaded.");
            }

            if (data == null)
                throw new ArgumentNullException("data");

            WriteRawFloatBlock(
                _wf2Data,
                _frontFloatOffset,
                data.FrontValues);

            WriteRawFloatBlock(
                _wf2Data,
                _rearFloatOffset,
                data.RearValues);

            /*
             * The loaded data is already decompressed and begins with 01.
             * Saving retains that valid uncompressed type-01 form.
             */
            File.WriteAllBytes(outputPath, _wf2Data);

            CurrentWf2Path = outputPath;
        }

        private static int FindVesu(
            byte[] data,
            int expectedVersion)
        {
            int searchOffset = 0;

            while (true)
            {
                int tagOffset = BinaryReaderUtil.FindSequence(
                    data,
                    BinaryReaderUtil.VesuTag,
                    searchOffset);

                if (tagOffset < 0)
                    break;

                int versionOffset = tagOffset + 4;

                if (versionOffset + 4 <= data.Length)
                {
                    int version = BinaryReaderUtil.ReadInt32(
                        data,
                        versionOffset);

                    if (version == expectedVersion)
                        return tagOffset;
                }

                searchOffset = tagOffset + 1;
            }

            throw new Exception(
                "Could not find usev version " + expectedVersion +
                ". This does not appear to be the expected suspension file.");
        }

        private static int[] FindLsevFloatOffsets(
            byte[] data,
            int searchOffset,
            int expectedVersion,
            int floatCount)
        {
            var offsets = new List<int>();

            while (offsets.Count < 2)
            {
                int tagOffset = BinaryReaderUtil.FindSequence(
                    data,
                    BinaryReaderUtil.LsevTag,
                    searchOffset);

                if (tagOffset < 0)
                    break;

                int versionOffset = tagOffset + 4;
                int countOffset = tagOffset + 8;
                int floatOffset = tagOffset + 12;

                bool hasEnoughData =
                    floatOffset + (floatCount * 4) <= data.Length;

                if (hasEnoughData)
                {
                    int version = BinaryReaderUtil.ReadInt32(
                        data,
                        versionOffset);

                    int count = BinaryReaderUtil.ReadInt32(
                        data,
                        countOffset);

                    if (version == expectedVersion && count == 1)
                        offsets.Add(floatOffset);
                }

                searchOffset = tagOffset + 1;
            }

            if (offsets.Count != 2)
            {
                throw new Exception(
                    "Could not locate both lsev suspension blocks. " +
                    "Expected two lsev version " +
                    expectedVersion +
                    " blocks with item count 1.");
            }

            return offsets.ToArray();
        }

        private static SuspensionData ReadWf2SuspensionData(
            byte[] data,
            int frontOffset,
            int rearOffset)
        {
            float[] front = ReadRawFloatBlock(
                data,
                frontOffset,
                SuspensionMapper.Wf2FloatCountPerAxle);

            float[] rear = ReadRawFloatBlock(
                data,
                rearOffset,
                SuspensionMapper.Wf2FloatCountPerAxle);

            return new SuspensionData(front, rear);
        }

        private static float[] ReadRawFloatBlock(
            byte[] data,
            int offset,
            int count)
        {
            float[] values = new float[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = BinaryReaderUtil.ReadSingle(
                    data,
                    offset + (i * 4));
            }

            return values;
        }

        private static void WriteRawFloatBlock(
            byte[] data,
            int offset,
            float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            for (int i = 0; i < values.Length; i++)
            {
                BinaryReaderUtil.WriteSingle(
                    data,
                    offset + (i * 4),
                    values[i]);
            }
        }
    }
}