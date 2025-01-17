using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTT.FormConnect.Services
{
    public class MqttService
    {
        private IMqttClient mqttClient;
        private Action<string> messageCallback;

        public async Task Start(string brokerIp, string clientId, Action<string> callback = null)
        {
            var factory = new MqttFactory();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(brokerIp)
            .WithClientId(clientId)
            .Build();

            mqttClient = factory.CreateMqttClient();
            messageCallback = callback;

            mqttClient.ConnectedAsync += async e =>
            {
                messageCallback?.Invoke("MQTT connected");
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("my/topic/receive").Build());
            };

            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                messageCallback?.Invoke($"Received MQTT message\n - Topic = {e.ApplicationMessage.Topic}\n - Payload = {payload}\n - QoS = {e.ApplicationMessage.QualityOfServiceLevel}\n - Retain = {e.ApplicationMessage.Retain}");
            };

            mqttClient.DisconnectedAsync += async e =>
            {
                messageCallback?.Invoke("MQTT reconnecting");
                await Task.Delay(TimeSpan.FromSeconds(5));
                await mqttClient.ConnectAsync(options, CancellationToken.None);
            };

            await mqttClient.ConnectAsync(options, CancellationToken.None);
        }

        public async Task SendCode(string message)
        {
            var topic = "my/topic/send";
            var applicationMessage = new MqttApplicationMessageBuilder()
              .WithTopic(topic)
              .WithPayload(message)
              .Build();
            await mqttClient.PublishAsync(applicationMessage);
        }
    }
}
