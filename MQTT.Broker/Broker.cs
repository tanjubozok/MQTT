using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace MQTT.Broker;

public class Broker
{
    private IMqttServer _mqttServer;
    private MqttServerOptionsBuilder _mqttServerOptionsBuilder;

    public Broker()
    {
        _mqttServerOptionsBuilder = new MqttServerOptionsBuilder()
            .WithConnectionValidator(c =>
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} " +
                    $"Endpoint: {c.Endpoint}  ==> ClientId {c.ClientId}");
                
                if (c.Username == "kullanici_adi" && c.Password == "sifre")
                    c.ReasonCode = MqttConnectReasonCode.Success;
                else
                    c.ReasonCode = MqttConnectReasonCode.NotAuthorized;
            })
            .WithApplicationMessageInterceptor(context =>
            {
                Console.WriteLine($"Id: {context.ClientId} ==>  " +
                    $"\ntopic: {context.ApplicationMessage.Topic} " +
                    $"\nPayload==> {Encoding.UTF8.GetString(context.ApplicationMessage.Payload)}");
            })
            .WithConnectionBacklog(1000)
            .WithDefaultEndpointBoundIPAddress(System.Net.IPAddress.Parse("127.0.0.1"))
            .WithDefaultEndpointPort(1884);
    }

    public void Start()
    {
        _mqttServer = new MqttFactory().CreateMqttServer();
        _mqttServer.StartAsync(_mqttServerOptionsBuilder.Build()).Wait();

        Console.WriteLine($"Mqtt Broker Oluşturuldu: " +
            $"Host: {_mqttServer.Options.DefaultEndpointOptions.BoundInterNetworkAddress} " +
            $"Port: {_mqttServer.Options.DefaultEndpointOptions.Port}");

        Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();
    }
    public void Stop()
    {
        _mqttServer.StopAsync().Wait();
    }
}