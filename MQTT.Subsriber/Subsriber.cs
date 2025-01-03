using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;

namespace MQTT.Subscriber;

public class Subscriber
{
    private IMqttClient _client;
    private IMqttClientOptions _options;
    private string _clientId;

    public Subscriber()
    {
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

        _clientId = $"Subscriber-{Guid.NewGuid()}";

        _options = new MqttClientOptionsBuilder()
            .WithClientId(_clientId) 
            .WithTcpServer("127.0.0.1", 1884)
            .WithCredentials("kullanici_adi", "sifre")
            .WithCleanSession()
            .Build();

        _client.UseConnectedHandler(e =>
        {
            Console.WriteLine($"MQTT Broker'a bağlanıldı. ClientId: {_clientId}");
            AboneOl();
        });

        _client.UseDisconnectedHandler(e =>
        {
            Console.WriteLine($"MQTT Broker'dan bağlantı kesildi... ClientId: {_clientId}");
        });

        _client.UseApplicationMessageReceivedHandler(e =>
        {
            string topic = e.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            string clientId = payload.Split(',')[0].Replace("ClientId: ", "").Trim();
            string mesaj = payload.Split(',')[1].Replace("Mesaj: ", "").Trim();

            Console.WriteLine($"Gelen Mesaj: \nTopic: {topic}" +
                $"\nClientId: {clientId}" +
                $"\nMesaj: {mesaj}\n");
        });
    }

    public void Basla()
    {
        _client.ConnectAsync(_options).Wait();
    }

    private void AboneOl()
    {
        _client.SubscribeAsync(new MqttTopicFilterBuilder()
            .WithTopic("topic")
            .WithExactlyOnceQoS()
            .Build()).Wait();

        Console.WriteLine($"ClientId: {_clientId} - 'Topic' abone olundu.");
    }

    public void BaglantiyiKes()
    {
        _client.DisconnectAsync().Wait();
        Console.WriteLine($"MQTT Broker ile bağlantı kesildi. ClientId: {_clientId}");
    }
}