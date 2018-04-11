using System;
using adrilight.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace adrilight.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        [TestMethod]
        public void DesignTimeCreation_Works()
        {
            var kernel = App.SetupDependencyInjection(true);

            var UserSettings = kernel.Get<IUserSettings>();
            Assert.IsNotNull(UserSettings, "UserSettings");

            var settingsViewModel = kernel.Get<SettingsViewModel>();
            Assert.IsNotNull(settingsViewModel, "settingsViewModel");
        }

        [TestMethod]
        public void RunTimeCreation_Works()
        {
            var kernel = App.SetupDependencyInjection(false);

            var UserSettings = kernel.Get<IUserSettings>();
            Assert.IsNotNull(UserSettings, "UserSettings");

            var settingsViewModel = kernel.Get<SettingsViewModel>();
            Assert.IsNotNull(settingsViewModel, "settingsViewModel");
        }
    }
}
