using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace MQTT.Publisher;

public class Publisher
{
    private readonly IMqttClient _client;
    private readonly IMqttClientOptions _options;

    static void Main(string[] args)
    {
        Publisher publisher = new();
        publisher.MesajGonder();
    }

    public Publisher()
    {
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithClientId($"Publisher-{Guid.NewGuid()}")
            .WithTcpServer("127.0.0.1", 1884)
            .WithCredentials("kullanici_adi", "sifre")
            .WithCleanSession()
            .Build();

        _client.UseConnectedHandler(e =>
        {
            Console.WriteLine("MQTT Broker ile bağlantı başarılı bir şekilde kuruldu.");
        });

        _client.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("MQTT Broker bağlantısı başarıyla sonlandırıldı.");
        });

        _client.ConnectAsync(_options).Wait();
        MesajGonder();
        Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();
    }

    public void MesajGonder()
    {
        while (true)
        {
            Console.Write("Mesajınızı yazıp gönderin: ");
            var message = Console.ReadLine();

            var payload = $"ClientId: {_options.ClientId}, Mesaj: {message}";

            var testMessage = new MqttApplicationMessageBuilder()
                .WithTopic("topic")
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag(false)
                .Build();

            if (_client.IsConnected)
            {
                Console.WriteLine($"Mesaj tarihi: {DateTime.UtcNow}, " +
                    $"Topic: {testMessage.Topic}, " +
                    $"Payload: {payload}");

                _client.PublishAsync(testMessage);
            }
        }
    }

    void BaglantiyiKes()
    {
        _client.DisconnectAsync().Wait();
        Console.WriteLine("Broker ile bağlantı kesildi...");
    }
}