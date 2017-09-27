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
        public Rectangle ExpectedScreenBound { get; } = new Rectangle(0,0,1920,1080);

        public ISpot[] Spots { get; set; } = new[] { new Spot(10, 20, 40, 40) };

        public object Lock { get; } = new object();

        public int CountLeds(int spotsX, int spotsY)
        {
            return Spots.GetLength(0);
        }

        public void IndicateMissingValues()
        {
        }
    }
}
