using adrilight.Resources;
using Newtonsoft.Json;
using NLog;
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
            if (App.IsPrivateBuild) return;

#if !DEBUG
            var t = new Thread(async () => await StartSquirrel())
            {
                Name = "adrilight Update Checker",
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            t.Start();
#endif
        }

        public IUserSettings Settings { get; }
        public IContext Context { get; }

        private async Task StartSquirrel()
        {
            while (true)
            {
                try
                {
                    //using (var mgr = new UpdateManager(ADRILIGHT_RELEASES))
                    //{
                    //    var releaseEntry = await mgr.UpdateApp();

                    //    if (releaseEntry != null)
                    //    {
                    //        //restart adrilight if an update was installed
                    //        UpdateManager.RestartApp();
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    _log.Error(ex, $"error when update checking: {ex.GetType().FullName}: {ex.Message}");
                }

                //check once a day for updates
                await Task.Delay(TimeSpan.FromDays(1));
            }
        }
    }
}
