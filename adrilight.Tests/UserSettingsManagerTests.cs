using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace adrilight.Tests
{
    [TestClass]
    public class UserSettingsManagerTests
    {
        [TestMethod]
        public void Save_and_Load_work()
        {
            var manager = new UserSettingsManager();

            var settings = manager.LoadIfExists() ?? manager.MigrateOrDefault();

            var b = (byte)(new Random().NextDouble()*255);

            settings.WhitebalanceBlue = b;
            //save should happen automatically!

            var settings2 = manager.LoadIfExists();
            Assert.AreEqual(b, settings.WhitebalanceBlue, "settings.WhitebalanceBlue");
        }

        [TestMethod]
        public void Migration_works()
        {
            var manager = new UserSettingsManager();

            var settings = manager.MigrateOrDefault();
        }
    }
}
