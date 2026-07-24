using STRmodsWF2SuspensionEditor.Models;

namespace STRmodsWF2SuspensionEditor.Services
{
    public static class SuspensionGeometryMapper
    {
        public const int FloatCountPerAxle = 31;

        public static readonly SuspensionField[] Fields =
        {
            new SuspensionField("Spindle Upper Arm X", 0, false),
            new SuspensionField("Spindle Upper Arm Y", 1, false),
            new SuspensionField("Spindle Upper Arm Z", 2, false),

            new SuspensionField("Body Upper Front Arm X", 3, false),
            new SuspensionField("Body Upper Front Arm Y", 4, false),
            new SuspensionField("Body Upper Front Arm Z", 5, false),

            new SuspensionField("Body Upper Rear Arm X", 6, false),
            new SuspensionField("Body Upper Rear Arm Y", 7, false),
            new SuspensionField("Body Upper Rear Arm Z", 8, false),

            new SuspensionField("Spindle Lower Arm X", 9, false),
            new SuspensionField("Spindle Lower Arm Y", 10, false),
            new SuspensionField("Spindle Lower Arm Z", 11, false),

            new SuspensionField("Body Lower Front Arm X", 12, false),
            new SuspensionField("Body Lower Front Arm Y", 13, false),
            new SuspensionField("Body Lower Front Arm Z", 14, false),

            new SuspensionField("Body Lower Rear Arm X", 15, false),
            new SuspensionField("Body Lower Rear Arm Y", 16, false),
            new SuspensionField("Body Lower Rear Arm Z", 17, false),

            new SuspensionField("Spindle Steering Rod X", 18, false),
            new SuspensionField("Spindle Steering Rod Y", 19, false),
            new SuspensionField("Spindle Steering Rod Z", 20, false),

            new SuspensionField("Body Steering Rod X", 21, false),
            new SuspensionField("Body Steering Rod Y", 22, false),
            new SuspensionField("Body Steering Rod Z", 23, false),

            new SuspensionField("Spindle Push Rod X", 24, false),
            new SuspensionField("Spindle Push Rod Y", 25, false),
            new SuspensionField("Spindle Push Rod Z", 26, false),

            new SuspensionField("Body Push Rod X", 27, false),
            new SuspensionField("Body Push Rod Y", 28, false),
            new SuspensionField("Body Push Rod Z", 29, false),

            new SuspensionField("Body Y-Offset", 30, false)
        };
    }
}