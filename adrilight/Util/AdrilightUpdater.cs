using adrilight.Resources;
using Newtonsoft.Json;
using NLog;
using Semver;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace adrilight.Util
{
    class AdrilightUpdater
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        private const string ADRILIGHT_RELEASES = "https://fabse.net/adrilight/Releases";

        public AdrilightUpdater(IUserSettings settings, IContext context)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void StartThread()
        {
            var t = new Thread(async () => await StartSquirrel())
            {
                Name = "adrilight Update Checker",
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            t.Start();
        }

        public IUserSettings Settings { get; }
        public IContext Context { get; }

        private async Task StartSquirrel()
        {
            using (var mgr = new UpdateManager(ADRILIGHT_RELEASES))
            {
                var releaseEntry = await mgr.UpdateApp();


            //Context.Invoke(() =>
            //{
            //    string message = $"New version of adrilight is available! The new version is {latestVersionNumber} (you are running {App.VersionNumber}). Press OK to open the download page!";
            //    const string title = "New Adrilight Version!";
            //    var shouldOpenUrl = MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK;

            //    if (shouldOpenUrl && latestRelease.LatestVersionUrl != null)
            //    {
            //        Process.Start(latestRelease.LatestVersionUrl);
            //    }
            //});

            }

        }
    }
}
