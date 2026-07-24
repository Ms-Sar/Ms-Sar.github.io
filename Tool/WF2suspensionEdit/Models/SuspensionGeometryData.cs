using System;

namespace STRmodsWF2SuspensionEditor.Models
{
    public sealed class SuspensionGeometryData
    {
        public float[] FrontValues { get; private set; }
        public float[] RearValues { get; private set; }

        public SuspensionGeometryData()
        {
            FrontValues = new float[31];
            RearValues = new float[31];
        }

        public SuspensionGeometryData(float[] frontValues, float[] rearValues)
        {
            if (frontValues == null || frontValues.Length != 31)
                throw new ArgumentException("Front geometry must contain 31 float values.");

            if (rearValues == null || rearValues.Length != 31)
                throw new ArgumentException("Rear geometry must contain 31 float values.");

            FrontValues = (float[])frontValues.Clone();
            RearValues = (float[])rearValues.Clone();
        }
    }
}