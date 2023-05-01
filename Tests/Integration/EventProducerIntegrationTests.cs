using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using PocKafka;
using PocKafka.Events;
using Xunit;

namespace PocKafka.Tests.Integration;

public class EventProducerIntegrationTests : IClassFixture<KafkaTestServerFixture>
{
    private readonly EventProducer _eventProducer;
    private readonly ProducerConfig _producerConfig;
    private readonly ConsumerConfig _consumerConfig;

    public EventProducerIntegrationTests(KafkaTestServerFixture fixture)
    {
        _eventProducer = fixture.ServiceProvider.GetRequiredService<EventProducer>();
        _consumerConfig = fixture.ServiceProvider.GetRequiredService<ConsumerConfig>();
        _producerConfig = fixture.ServiceProvider.GetRequiredService<ProducerConfig>();
    }

    [Fact]
    public async Task TestProduceAsyncIntegration()
    {
        // Arrange
        var testTopicName = $"test-{Guid.NewGuid()}";
        PostCreatedEvent eventData = new(); // Replace with your actual derived event class

        try
        {
            // Act
            await _eventProducer.ProduceAsync(testTopicName, eventData);

            // Assert
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(testTopicName);
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(10));
            consumer.Close();

            PostCreatedEvent consumedEventData = JsonSerializer.Deserialize<PostCreatedEvent>(consumeResult.Message.Value);

            Assert.NotNull(consumeResult);
            Assert.Equal(eventData, consumedEventData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception on TestProduceAsyncIntegration{ex.Message}");
        }
        finally
        {
            // Clean up the test topic
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _producerConfig.BootstrapServers }).Build())
            {
                await adminClient.DeleteTopicsAsync(new List<string> { testTopicName });
            }
        }
    }
}