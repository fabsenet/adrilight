using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Buffers;
using System.Windows.Media;
using adrilight.Util;
using System.Linq;
using Newtonsoft.Json;
using MQTTnet;
using System.Text;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client.Options;

namespace adrilight
{
    internal sealed class Smarthome : ISmarthome
    {
        private ILogger _log = LogManager.GetCurrentClassLogger();

        private IUserSettings UserSettings { get; }
        private CancellationToken _cancellationToken;

        public Smarthome(IUserSettings userSettings, CancellationToken cancellationToken)
        {
            UserSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
            _cancellationToken = cancellationToken;
            UserSettings.PropertyChanged += UserSettings_PropertyChanged;
            _log.Info($"Smarthome created.");
        }

        private void UserSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(UserSettings.TransferActive):
                    break;
            }
        }

        public async Task DoWorkAsync()
        {
            using var mqttClient = new MqttFactory().CreateManagedMqttClient();
            var basetopic = $"{Environment.MachineName}/adrilight";

            var options = new ManagedMqttClientOptionsBuilder()
                            .WithClientOptions(new MqttClientOptionsBuilder()
                                                .WithTcpServer("MQTT-SERVER-TODO")
                                                .WithCommunicationTimeout(TimeSpan.FromSeconds(1))
                                                .WithKeepAlivePeriod(TimeSpan.FromSeconds(1))
                                                //.WithCredentials("bud", "%spencer%")
                                                //.WithCleanSession()
                                                .WithWillMessage(new MqttApplicationMessage {
                                                    Retain = true,
                                                    Topic = $"{basetopic}/state",
                                                    Payload = Encoding.UTF8.GetBytes("Offline")
                                                })
                                                .Build())
                
                            .Build();

            mqttClient.UseConnectedHandler(async e =>
            {
                if (e.AuthenticateResult.ResultCode == MQTTnet.Client.Connecting.MqttClientConnectResultCode.Success)
                {
                    await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                        .WithAtLeastOnceQoS()
                        .WithRetainFlag()
                        .WithTopic($"{basetopic}/state")
                        .WithPayload("Online")
                        .Build());
                }
            });

            await mqttClient.StartAsync(options);

            await Task.Delay(-1, _cancellationToken);
        }

    }
}