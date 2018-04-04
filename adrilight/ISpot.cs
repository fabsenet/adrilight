using System.Drawing;
using Color = System.Windows.Media.Color;

namespace adrilight
{
    public interface ISpot
    {
        byte Red { get; }
        byte Green { get; }
        byte Blue { get; }

        Color OnDemandColor { get; }
        Rectangle Rectangle { get; }

        void IndicateMissingValue();
        void SetColor(byte red, byte green, byte blue);
    }
}