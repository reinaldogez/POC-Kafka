using System;
using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;
using Xunit;
using PocKafka.Utils;

namespace PocKafka.Tests.Integration;

public class KafkaTestServerFixture
{
    public ServiceProvider ServiceProvider { get; private set; }
    public KafkaTestServerFixture()
    {
        var services = new ServiceCollection();
        
        ProducerConfig configProducer = ConfigFiles.LoadConfig<ProducerConfig>("kafkaproducer.config").Result;
        ConsumerConfig configConsumer = ConfigFiles.LoadConfig<ConsumerConfig>("kafkaconsumer.config").Result;
        services.AddSingleton<ProducerConfig>(configProducer);
        services.AddSingleton<ConsumerConfig>(configConsumer);
        services.AddScoped<EventProducer>();

        ServiceProvider = services.BuildServiceProvider();
    }

}
