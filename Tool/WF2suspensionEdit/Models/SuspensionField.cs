namespace STRmodsWF2SuspensionEditor.Models
{
    public sealed class SuspensionField
    {
        public string Name { get; private set; }
        public int FloatIndex { get; private set; }
        public bool IsCamberAngle { get; private set; }

        public SuspensionField(string name, int floatIndex, bool isCamberAngle)
        {
            Name = name;
            FloatIndex = floatIndex;
            IsCamberAngle = isCamberAngle;
        }
    }
}