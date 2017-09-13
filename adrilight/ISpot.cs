using System.Drawing;

namespace adrilight
{
    public interface ISpot
    {
        byte Red { get; }
        byte Green { get; }
        byte Blue { get; }

        SolidBrush OnDemandBrush { get; }
        Rectangle Rectangle { get; }
        Rectangle RectangleOverlayBorder { get; }
        Rectangle RectangleOverlayFilling { get; }

        void IndicateMissingValue();
        void SetColor(byte red, byte green, byte blue);
    }
}