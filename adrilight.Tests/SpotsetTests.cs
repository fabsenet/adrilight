using System;
using System.Drawing;
using adrilight.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace adrilight.Tests
{
    [TestClass]
    public class SpotsetTests
    {
        [TestMethod]
        public void SimpleSwap_Works()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            arr.Swap(1, 1);
            Assert.AreEqual(2, arr[1], "self swap");

            arr.Swap(1, 2);
            CollectionAssert.AreEqual(new[] { 1, 3, 2, 4, 5 }, arr, "real swap");
        }

        [TestMethod]
        public void BuildSpotset_Works1()
        {
            var userSettings = new Mock<IUserSettings>(MockBehavior.Strict);
            userSettings
                .SetupProperty(s => s.SpotsX, 1)
                .SetupProperty(s => s.SpotsY, 1)
                .SetupProperty(s => s.OffsetX, 0)
                .SetupProperty(s => s.OffsetY, 0)
                .SetupProperty(s => s.BorderDistanceX, 0)
                .SetupProperty(s => s.BorderDistanceY, 0)
                .SetupProperty(s => s.OffsetLed, 0)
                .SetupProperty(s => s.SpotWidth, 90)
                .SetupProperty(s => s.SpotHeight, 80)
                ;
            var spotset = new SpotSet(userSettings.Object);

            Assert.IsNotNull(spotset.Spots, "spotset.Spots");
            Assert.AreEqual(1, spotset.Spots.Length, "spotset.Spots.Length");
            Assert.AreEqual(new Rectangle(0,0,90,80), spotset.Spots[0].Rectangle, "spotset.Spots[0].Rectangle");
        }
    }
}
