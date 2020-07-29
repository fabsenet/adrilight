using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Fakes
{
    class SpotSetFake : ISpotSet
    {
        public SpotSetFake()
        {
            Spots = Enumerable.Range(0, 10)
                .Select(i => new Spot(i * 20, i * 15, 15, 10))
                .ToArray();

            Spots[0].IsFirst = true;
        }

        public ISpot[] Spots { get; set; }

        public object Lock { get; } = new object();

        public int ExpectedScreenWidth => 1920;

        public int ExpectedScreenHeight => 1080;

        public int CountLeds(int spotsX, int spotsY)
        {
            return Spots.GetLength(0);
        }

        public void IndicateMissingValues()
        {
        }
    }
}
