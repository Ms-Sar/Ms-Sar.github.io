using System;

namespace STRmodsWF2SuspensionEditor.Models
{
    public sealed class SuspensionData
    {
        public const int Wf2FloatCountPerAxle = 23;

        public float[] FrontValues { get; private set; }
        public float[] RearValues { get; private set; }

        public SuspensionData()
        {
            FrontValues = new float[Wf2FloatCountPerAxle];
            RearValues = new float[Wf2FloatCountPerAxle];
        }

        public SuspensionData(float[] frontValues, float[] rearValues)
        {
            if (frontValues == null ||
                frontValues.Length != Wf2FloatCountPerAxle)
            {
                throw new ArgumentException(
                    "Front suspension must contain 23 values.");
            }

            if (rearValues == null ||
                rearValues.Length != Wf2FloatCountPerAxle)
            {
                throw new ArgumentException(
                    "Rear suspension must contain 23 values.");
            }

            FrontValues = (float[])frontValues.Clone();
            RearValues = (float[])rearValues.Clone();
        }
    }
}