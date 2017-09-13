using System.Drawing;

namespace adrilight
{
    public interface ISpotSet
    {
        Rectangle ExpectedScreenBound { get; }
        ISpot[] Spots { get; set; }
        object Lock { get; }

        int CountLeds(int spotsX, int spotsY);
        void IndicateMissingValues();
    }
}