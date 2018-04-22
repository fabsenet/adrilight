using adrilight.Resources;
using Microsoft.ApplicationInsights;
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
        private readonly TelemetryClient tc;
        private const string ADRILIGHT_RELEASES = "https://fabse.net/adrilight/Releases";

        public AdrilightUpdater(IUserSettings settings, IContext context, TelemetryClient tc)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            this.tc = tc ?? throw new ArgumentNullException(nameof(tc));
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
            try
            {

            using (var mgr = new UpdateManager(ADRILIGHT_RELEASES))
            {
                var releaseEntry = await mgr.UpdateApp();

                    //TODO notify user about update to restart adrilight?!

            //Context.Invoke(() =>
            //{
            //    string message = $"New version of adrilight is available! The new version is {latestVersionNumber} (you are running {App.VersionNumber}). Press OK to open the download page!";
            //    const string title = "New Adrilight Version!";
            //    var shouldOpenUrl = MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
            //});

            }

            }
            catch (Exception ex)
            {
                tc.TrackException(ex);
            }
        }
    }
}
