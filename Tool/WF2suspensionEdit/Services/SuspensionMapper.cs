using System;
using STRmodsWF2SuspensionEditor.Models;

namespace STRmodsWF2SuspensionEditor.Services
{
    public static class SuspensionMapper
    {
        public const int Wf1FloatCountPerAxle = 22;
        public const int Wf2FloatCountPerAxle = 23;

        public const int Wf2UnknownFieldIndex = 5;

        public static readonly SuspensionField[] Fields =
        {
            new SuspensionField("Ride Height", 0, false),
            new SuspensionField("Bump Stop Up", 1, false),
            new SuspensionField("Bump Stop Down", 2, false),
            new SuspensionField("Spring Rate", 3, false),
            new SuspensionField("Progressive Rate", 4, false),

            new SuspensionField("???", 5, false),

            new SuspensionField("Bump Stop Length", 6, false),
            new SuspensionField("Bump Stop Rate", 7, false),
            new SuspensionField("Bump Stop Damp", 8, false),

            new SuspensionField(
                "Bump Stop Rate Gain (Deflection Squared)",
                9,
                false),

            new SuspensionField(
                "Bump Stop Damp Gain (Deflection Squared)",
                10,
                false),

            new SuspensionField("Rebound Length", 11, false),
            new SuspensionField("Rebound Rate", 12, false),

            new SuspensionField("Bump Limits X", 13, false),
            new SuspensionField("Bump Limits Y", 14, false),

            new SuspensionField("Bump Damp X", 15, false),
            new SuspensionField("Bump Damp Y", 16, false),

            new SuspensionField("Rebound Limits X", 17, false),
            new SuspensionField("Rebound Limits Y", 18, false),

            new SuspensionField("Rebound Damp X", 19, false),
            new SuspensionField("Rebound Damp Y", 20, false),

            new SuspensionField("Rollbar Stiffness", 21, false),
            new SuspensionField("Camber Angle (Degrees)", 22, true)
        };

        public static float ToDisplayValue(
            SuspensionField field,
            float storedValue)
        {
            if (!field.IsCamberAngle)
                return storedValue;

            return storedValue * (180.0f / (float)Math.PI);
        }

        public static float ToStoredValue(
            SuspensionField field,
            float displayValue)
        {
            if (!field.IsCamberAngle)
                return displayValue;

            return displayValue * ((float)Math.PI / 180.0f);
        }

        public static float[] ConvertWf1ValuesToWf2Values(
            float[] wf1Values)
        {
            if (wf1Values == null ||
                wf1Values.Length != Wf1FloatCountPerAxle)
            {
                throw new ArgumentException(
                    "WF1 suspension data must contain 22 values.");
            }

            float[] wf2Values = new float[Wf2FloatCountPerAxle];

            for (int wf2Index = 0;
                 wf2Index < Wf2FloatCountPerAxle;
                 wf2Index++)
            {
                if (wf2Index == Wf2UnknownFieldIndex)
                {
                    /*
                     * WF1 has no corresponding float here.
                     * NaN tells the UI to preserve the WF2 value
                     * currently displayed in the ??? field.
                     */
                    wf2Values[wf2Index] = float.NaN;
                    continue;
                }

                int wf1Index = wf2Index;

                if (wf2Index > Wf2UnknownFieldIndex)
                    wf1Index--;

                wf2Values[wf2Index] = wf1Values[wf1Index];
            }

            return wf2Values;
        }
    }
}