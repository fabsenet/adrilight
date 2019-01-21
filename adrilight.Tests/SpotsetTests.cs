using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
                .SetupProperty(s => s.BorderDistanceX, 0)
                .SetupProperty(s => s.BorderDistanceY, 0)
                .SetupProperty(s => s.OffsetLed, 0)
                .SetupProperty(s => s.SpotWidth, 90)
                .SetupProperty(s => s.SpotHeight, 80)
                ;
            var spots = SpotSet.BuildSpots(180, 120, userSettings.Object);

            Assert.IsNotNull(spots, "spotset.Spots");
            Assert.AreEqual(4, spots.Length, "spotset.Spots.Length");
            Assert.AreEqual(new Rectangle(84, 0, 90/8, 80/8), spots[0].Rectangle, "spots[0].Rectangle");
        }

        [TestMethod]
        public void BuildSpotset_2x2()
        {
            var userSettings = new Mock<IUserSettings>(MockBehavior.Strict);
            userSettings
                .SetupProperty(s => s.SpotsX, 2)
                .SetupProperty(s => s.SpotsY, 2)
                .SetupProperty(s => s.BorderDistanceX, 1)
                .SetupProperty(s => s.BorderDistanceY, 1)
                .SetupProperty(s => s.OffsetLed, 0)
                .SetupProperty(s => s.SpotWidth, 10)
                .SetupProperty(s => s.SpotHeight, 10)
                .SetupProperty(s => s.MirrorX, false)
                .SetupProperty(s => s.MirrorY, false)
                ;

            var spots = SpotSet.BuildSpots(1920, 1080, userSettings.Object);

            Assert.IsNotNull(spots, "spotset.Spots");
            Assert.AreEqual(2+2+2+2, spots.Length, "spotset.Spots.Length");
            Assert.AreEqual(new Rectangle(639, 0, 1, 1), spots[0].Rectangle, "spotset.Spots[0].Rectangle");
        }

        [TestMethod]
        public void BuildSpotset_2x2_Offset1_works()
        {
            var userSettings = new Mock<IUserSettings>(MockBehavior.Strict);
            userSettings
                .SetupProperty(s => s.SpotsX, 2)
                .SetupProperty(s => s.SpotsY, 2)
                .SetupProperty(s => s.BorderDistanceX, 1)
                .SetupProperty(s => s.BorderDistanceY, 1)
                .SetupProperty(s => s.OffsetLed, 1)
                .SetupProperty(s => s.SpotWidth, 10)
                .SetupProperty(s => s.SpotHeight, 10)
                .SetupProperty(s => s.MirrorX, false)
                .SetupProperty(s => s.MirrorY, false)
                ;

            var spots = SpotSet.BuildSpots(1920, 1080, userSettings.Object);

            Assert.IsNotNull(spots, "spotset.Spots");
            Assert.AreEqual(2 + 2 + 2 + 2, spots.Length, "spotset.Spots.Length");
            Assert.AreEqual(new Rectangle(0, 359, 1, 1), spots[0].Rectangle, "spotset.Spots[0].Rectangle");
        }
        [TestMethod]
        public void BuildSpotset_2x2_Offset2_works()
        {
            var userSettings = new Mock<IUserSettings>(MockBehavior.Strict);
            userSettings
                .SetupProperty(s => s.SpotsX, 2)
                .SetupProperty(s => s.SpotsY, 2)
                .SetupProperty(s => s.BorderDistanceX, 1)
                .SetupProperty(s => s.BorderDistanceY, 1)
                .SetupProperty(s => s.OffsetLed, 2)
                .SetupProperty(s => s.SpotWidth, 10)
                .SetupProperty(s => s.SpotHeight, 10)
                .SetupProperty(s => s.MirrorX, false)
                .SetupProperty(s => s.MirrorY, false)
                ;

            var spots = SpotSet.BuildSpots(1920, 1080, userSettings.Object);

            Assert.IsNotNull(spots, "spotset.Spots");
            Assert.AreEqual(2 + 2 + 2 + 2, spots.Length, "spotset.Spots.Length");
            Assert.AreEqual(new Rectangle(0, 719, 1, 1), spots[0].Rectangle, "spotset.Spots[0].Rectangle");
        }

        //https://github.com/fabsenet/adrilight/issues/68
        [TestMethod]
        public void BuildSpotset_4k_BorderDistance_1()
        {
            var userSettings = new Mock<IUserSettings>(MockBehavior.Strict);
            userSettings
                .SetupProperty(s => s.SpotsX, 40)
                .SetupProperty(s => s.SpotsY, 25)
                .SetupProperty(s => s.BorderDistanceX, 1)
                .SetupProperty(s => s.BorderDistanceY, 1)
                .SetupProperty(s => s.OffsetLed, 0)
                .SetupProperty(s => s.SpotWidth, 28)
                .SetupProperty(s => s.SpotHeight, 30)
                .SetupProperty(s => s.MirrorX, false)
                .SetupProperty(s => s.MirrorY, false)
                ;

            var spots = SpotSet.BuildSpots(3840/8, 2160/8, userSettings.Object);

            Assert.IsNotNull(spots, "spotset.Spots");
            Assert.AreEqual(40+40+25+25, spots.Length, "spotset.Spots.Length");
            Assert.AreEqual(new Rectangle(11, 0, 28/8, 30/8), spots[0].Rectangle, "spotset.Spots[0].Rectangle");
        }

        [TestMethod]
        public void BoundsWalker_1_1()
        {
            var items = SpotSet.BoundsWalker(1, 1).ToList();
            Assert.AreEqual(4, items.Count, "items.Count");

            Assert.AreEqual((1, 0), items[0], "item 0");
            Assert.AreEqual((2, 1), items[1], "item 1");
            Assert.AreEqual((1, 2), items[2], "item 2");
            Assert.AreEqual((0, 1), items[3], "item 3");
        }


        [TestMethod]
        public void BoundsWalker_5_3()
        {
            var items = SpotSet.BoundsWalker(5, 3).ToList();
            Assert.AreEqual(5+5+3+3, items.Count, "items.Count");

            Assert.AreEqual(0, items.Select(i => i.x).Min(), "min x");
            Assert.AreEqual(6, items.Select(i => i.x).Max(), "max x");
            Assert.AreEqual(0, items.Select(i => i.y).Min(), "min y");
            Assert.AreEqual(4, items.Select(i => i.y).Max(), "max y");

            Assert.AreEqual((1, 0), items[0], "item 0");
            Assert.AreEqual((2, 0), items[1], "item 1");
            Assert.AreEqual((3, 0), items[2], "item 2");
            Assert.AreEqual((4, 0), items[3], "item 3");
            Assert.AreEqual((5, 0), items[4], "item 4");
            Assert.AreEqual((6, 1), items[5], "item 5");
            Assert.AreEqual((6, 2), items[6], "item 6");
            Assert.AreEqual((6, 3), items[7], "item 7");
            Assert.AreEqual((5, 4), items[8], "item 8");
            Assert.AreEqual((4, 4), items[9], "item 9");
            Assert.AreEqual((3, 4), items[10], "item 10");
            Assert.AreEqual((2, 4), items[11], "item 11");
            Assert.AreEqual((1, 4), items[12], "item 12");
            Assert.AreEqual((0, 3), items[13], "item 13");
            Assert.AreEqual((0, 2), items[14], "item 14");
            Assert.AreEqual((0, 1), items[15], "item 15");
        }


        [TestMethod]
        public void BoundsWalker_7_5()
        {
            var items = SpotSet.BoundsWalker(7, 5).ToList();
            Assert.AreEqual(7+7+5+5, items.Count, "items.Count");

            Assert.AreEqual(0, items.Select(i => i.x).Min(), "min x");
            Assert.AreEqual(8, items.Select(i => i.x).Max(), "max x");
            Assert.AreEqual(0, items.Select(i => i.y).Min(), "min y");
            Assert.AreEqual(6, items.Select(i => i.y).Max(), "max y");

            Assert.AreEqual((1, 0), items[0], "item 0");
            Assert.AreEqual((2, 0), items[1], "item 1");
            Assert.AreEqual((3, 0), items[2], "item 2");
            Assert.AreEqual((4, 0), items[3], "item 3");
            Assert.AreEqual((5, 0), items[4], "item 4");
            Assert.AreEqual((6, 0), items[5], "item 5");
            Assert.AreEqual((7, 0), items[6], "item 6");
            Assert.AreEqual((8, 1), items[7], "item 7");
            Assert.AreEqual((8, 2), items[8], "item 8");
            Assert.AreEqual((8, 3), items[9], "item 9");
            Assert.AreEqual((8, 4), items[10], "item 10");
            Assert.AreEqual((8, 5), items[11], "item 11");
            Assert.AreEqual((7, 6), items[12], "item 12");
            Assert.AreEqual((6, 6), items[13], "item 13");
            Assert.AreEqual((5, 6), items[14], "item 14");
            Assert.AreEqual((4, 6), items[15], "item 15");
            Assert.AreEqual((3, 6), items[16], "item 16");
            Assert.AreEqual((2, 6), items[17], "item 17");
            Assert.AreEqual((1, 6), items[18], "item 18");
            Assert.AreEqual((0, 5), items[19], "item 19");
            Assert.AreEqual((0, 4), items[20], "item 20");
            Assert.AreEqual((0, 3), items[21], "item 21");
            Assert.AreEqual((0, 2), items[22], "item 22");
            Assert.AreEqual((0, 1), items[23], "item 23");

            //for (int i = 0; i < items.Count; i++)
            //{
            //    var item = items[i];
            //    Debug.WriteLine($"Assert.AreEqual(({item.x}, {item.y}), items[{i}], \"item {i}\");");
            //}
        }
    }
}
