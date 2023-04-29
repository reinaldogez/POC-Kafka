using System.Text.Json;
using Confluent.Kafka;
using PocKafka.Events;

namespace PocKafka;

public class EventProducer
{
    private readonly ProducerConfig _config;

    public EventProducer(ProducerConfig config)
    {
        _config = config;
    }

    public async Task ProduceAsync<T>(string topic, T eventData) where T : BaseEvent
    {
        using var producer = new ProducerBuilder<string, string>(_config)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();

        var eventMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(eventData, eventData.GetType())
        };

        var deliveryResult = await producer.ProduceAsync(topic, eventMessage);

        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception($"Could not produce {eventData.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResult.Message}.");
        }
    }
}