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

            var testDate = DateTime.UtcNow;

            settings.LastUpdateCheck = testDate;
            //save should happen automatically!

            var settings2 = manager.LoadIfExists();
            Assert.AreEqual(testDate, settings.LastUpdateCheck, "settings.LastUpdateCheck");
        }

        [TestMethod]
        public void Migration_works()
        {
            var manager = new UserSettingsManager();

            var settings = manager.MigrateOrDefault();
        }
    }
}
