using adrilight.Resources;
using Newtonsoft.Json;
using NLog;
using Semver;
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

        public AdrilightUpdater(IUserSettings settings, IContext context)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void StartThread()
        {
            var t = new Thread(async () => await StartVersionCheck())
            {
                Name = "adrilight Update Checker",
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            t.Start();
        }

        public IUserSettings Settings { get; }
        public IContext Context { get; }

        private async Task StartVersionCheck()
        {
            //avoid too many checks
            if (Settings.LastUpdateCheck.HasValue && Settings.LastUpdateCheck > DateTime.UtcNow.AddHours(-8)) return;

            try
            {
                var latestRelease = await TryGetLatestReleaseData();
                if (latestRelease == null) return;

                string tagName = latestRelease.LatestVersionName;
                var latestVersionNumber = SemVersion.Parse(tagName.TrimStart('v', 'V'));

                Settings.LastUpdateCheck = DateTime.UtcNow;

                if (latestVersionNumber > App.VersionNumber)
                {
                    Context.Invoke(() =>
                    {
                        string message = $"New version of adrilight is available! The new version is {latestVersionNumber} (you are running {App.VersionNumber}). Press OK to open the download page!";
                        const string title = "New Adrilight Version!";
                        var shouldOpenUrl = MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK;

                        if (shouldOpenUrl && latestRelease.LatestVersionUrl != null)
                        {
                            Process.Start(latestRelease.LatestVersionUrl);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _log.Warn(ex, "Something failed in StartVersionCheck()");
                throw;
            }
        }

        [DebuggerDisplay("GithubReleaseData: {LatestVersionName} {LatestVersionUrl}")]
        private class GithubReleaseData
        {
            [JsonProperty("tag_name")]
            public string LatestVersionName { get; set; }

            [JsonProperty("html_url")]
            public string LatestVersionUrl { get; set; }
        }

        private async Task<GithubReleaseData> TryGetLatestReleaseData()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "fabsenet/adrilight");

                    var jsonString = await client.GetStringAsync("https://api.github.com/repos/fabsenet/adrilight/releases/latest");
                    var data = JsonConvert.DeserializeObject<GithubReleaseData>(jsonString);
                    return data;
                }
            }
            catch (Exception ex)
            {
                _log.Info(ex, "Check for latest release failed.");
                return null;
            }
        }

    }
}
