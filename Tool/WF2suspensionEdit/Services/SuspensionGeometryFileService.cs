using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using STRmodsWF2SuspensionEditor.Models;

namespace STRmodsWF2SuspensionEditor.Services
{
    public sealed class SuspensionGeometryFileService
    {
        private const int Wf1GeometryVersion = 3;
        private const int Wf2GeometryVersion = 4;

        private static readonly byte[] GsevTag =
            Encoding.ASCII.GetBytes("gsev");

        private static readonly byte[] DgevTag =
            Encoding.ASCII.GetBytes("dgev");

        private byte[] _wf2Data;
        private int _frontFloatOffset;
        private int _rearFloatOffset;

        public string CurrentWf2Path { get; private set; }

        public bool HasWf2File
        {
            get { return _wf2Data != null; }
        }

        public SuspensionGeometryData LoadWf2Geometry(string path)
        {
            byte[] originalData = File.ReadAllBytes(path);
            byte[] decompressed = Decompressor.Decompress(originalData);

            int gsevOffset = FindTagWithVersion(
                decompressed,
                GsevTag,
                Wf2GeometryVersion,
                0);

            int[] floatOffsets = FindWf2DgevBlocks(
                decompressed,
                gsevOffset + 8);

            _wf2Data = decompressed;
            _frontFloatOffset = floatOffsets[0];
            _rearFloatOffset = floatOffsets[1];
            CurrentWf2Path = path;

            return ReadGeometryData(
                _wf2Data,
                _frontFloatOffset,
                _rearFloatOffset);
        }

        public SuspensionGeometryData LoadWf1GeometryFromVesu(string path)
        {
            byte[] originalData = File.ReadAllBytes(path);
            byte[] decompressed = Decompressor.Decompress(originalData);

            int firstGsevOffset = FindTagWithVersion(
                decompressed,
                GsevTag,
                Wf1GeometryVersion,
                0);

            int[] floatOffsets = FindWf1GsevBlocks(
                decompressed,
                firstGsevOffset);

            return ReadGeometryData(
                decompressed,
                floatOffsets[0],
                floatOffsets[1]);
        }

        public void SaveWf2Geometry(
            string outputPath,
            SuspensionGeometryData data)
        {
            if (_wf2Data == null)
                throw new Exception("No Wreckfest 2 .vesg file has been loaded.");

            if (data == null)
                throw new ArgumentNullException("data");

            WriteGeometryData(
                _wf2Data,
                _frontFloatOffset,
                _rearFloatOffset,
                data);

            File.WriteAllBytes(outputPath, _wf2Data);
            CurrentWf2Path = outputPath;
        }

        private static int FindTagWithVersion(
            byte[] data,
            byte[] tag,
            int expectedVersion,
            int startOffset)
        {
            int searchOffset = startOffset;

            while (true)
            {
                int tagOffset = BinaryReaderUtil.FindSequence(
                    data,
                    tag,
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
                "Could not locate the expected geometry tag/version.");
        }

        private static int[] FindWf2DgevBlocks(
            byte[] data,
            int searchOffset)
        {
            var offsets = new List<int>();

            while (offsets.Count < 2)
            {
                int tagOffset = BinaryReaderUtil.FindSequence(
                    data,
                    DgevTag,
                    searchOffset);

                if (tagOffset < 0)
                    break;

                int versionOffset = tagOffset + 4;
                int countOffset = tagOffset + 8;
                int floatOffset = tagOffset + 12;

                bool hasEnoughData =
                    floatOffset +
                    (SuspensionGeometryMapper.FloatCountPerAxle * 4)
                    <= data.Length;

                if (hasEnoughData)
                {
                    int version = BinaryReaderUtil.ReadInt32(
                        data,
                        versionOffset);

                    int count = BinaryReaderUtil.ReadInt32(
                        data,
                        countOffset);

                    if (version == 0 && count == 1)
                        offsets.Add(floatOffset);
                }

                searchOffset = tagOffset + 1;
            }

            if (offsets.Count != 2)
            {
                throw new Exception(
                    "Could not locate both WF2 dgev geometry blocks. " +
                    "Expected two dgev version-0 blocks with item count 1.");
            }

            return offsets.ToArray();
        }

        private static int[] FindWf1GsevBlocks(
            byte[] data,
            int firstGsevOffset)
        {
            var offsets = new List<int>();
            int searchOffset = firstGsevOffset;

            while (offsets.Count < 2)
            {
                int tagOffset = BinaryReaderUtil.FindSequence(
                    data,
                    GsevTag,
                    searchOffset);

                if (tagOffset < 0)
                    break;

                int versionOffset = tagOffset + 4;
                int countOffset = tagOffset + 8;
                int floatOffset = tagOffset + 12;

                bool hasEnoughData =
                    floatOffset +
                    (SuspensionGeometryMapper.FloatCountPerAxle * 4)
                    <= data.Length;

                if (hasEnoughData)
                {
                    int version = BinaryReaderUtil.ReadInt32(
                        data,
                        versionOffset);

                    int count = BinaryReaderUtil.ReadInt32(
                        data,
                        countOffset);

                    if (version == Wf1GeometryVersion && count == 1)
                        offsets.Add(floatOffset);
                }

                searchOffset = tagOffset + 1;
            }

            if (offsets.Count != 2)
            {
                throw new Exception(
                    "Could not locate both WF1 gsev geometry blocks. " +
                    "Expected two gsev version-3 blocks with item count 1.");
            }

            return offsets.ToArray();
        }

        private static SuspensionGeometryData ReadGeometryData(
            byte[] data,
            int frontOffset,
            int rearOffset)
        {
            float[] front =
                new float[SuspensionGeometryMapper.FloatCountPerAxle];

            float[] rear =
                new float[SuspensionGeometryMapper.FloatCountPerAxle];

            for (int i = 0;
                 i < SuspensionGeometryMapper.FloatCountPerAxle;
                 i++)
            {
                front[i] = BinaryReaderUtil.ReadSingle(
                    data,
                    frontOffset + (i * 4));

                rear[i] = BinaryReaderUtil.ReadSingle(
                    data,
                    rearOffset + (i * 4));
            }

            return new SuspensionGeometryData(front, rear);
        }

        private static void WriteGeometryData(
            byte[] data,
            int frontOffset,
            int rearOffset,
            SuspensionGeometryData values)
        {
            for (int i = 0;
                 i < SuspensionGeometryMapper.FloatCountPerAxle;
                 i++)
            {
                BinaryReaderUtil.WriteSingle(
                    data,
                    frontOffset + (i * 4),
                    values.FrontValues[i]);

                BinaryReaderUtil.WriteSingle(
                    data,
                    rearOffset + (i * 4),
                    values.RearValues[i]);
            }
        }
    }
}