using System.Threading.Tasks;
using MQTTnet.Client.Disconnecting;

namespace adrilight
{
    internal class AdrilightDisconnectedHandler : IMqttClientDisconnectedHandler
    {
        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}