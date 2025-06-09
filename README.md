# MQTT

Bu proje, C# ve MQTTnet kütüphanesi kullanılarak hazırlanmış basit bir MQTT mesajlaşma altyapısı örneğidir. Konsol ve Windows Forms arayüzleri ile MQTT Broker, Publisher (yayıncı) ve Subscriber (abone) rollerini örneklemektedir.

## Özellikler

- Yerel veya uzak bir MQTT broker başlatabilir ve yönetebilirsiniz.
- Konsol üzerinden mesaj gönderme (Publisher) ve dinleme (Subscriber) örnekleri.
- Form tabanlı istemci ile görsel MQTT bağlantısı ve mesajlaşma.
- Basit kimlik doğrulama, bağlantı, abonelik, mesaj gönderimi ve mesaj alımı işlemleri.
- Abone ve yayıncılar için örnek kodlar.
- Broker üzerinde bağlantı ve mesaj günlüklemesi.

## Yapı

- `MQTT.Broker`: MQTT sunucu (broker) uygulaması.
- `MQTT.Publisher`: Konsol tabanlı mesaj gönderici (publisher) istemcisi.
- `MQTT.Subscriber`: Konsol tabanlı mesaj dinleyici (subscriber) istemcisi.
- `MQTT.FormConnect`: Windows Forms tabanlı MQTT istemci arayüzü.

## Kullanım

1. Broker uygulamasını başlatın (`MQTT.Broker`).
2. Publisher veya Subscriber uygulamalarını çalıştırarak broker ile iletişime geçin.
3. Publisher konsolundan mesaj gönderin, subscriber konsolunda mesajlar anlık olarak görüntülenecektir.
4. İsteğe bağlı olarak FormConnect projesini kullanarak görsel arayüz üzerinden bağlantı ve mesajlaşma sağlayabilirsiniz.

## Gereksinimler

- .NET 6 veya üzeri
- [MQTTnet](https://github.com/dotnet/MQTTnet) NuGet paketi

## Lisans

MIT Lisansı
