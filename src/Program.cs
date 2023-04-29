using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PocKafka.Events;
using PocKafka.Utils;

namespace PocKafka;
class Program
{
    static async Task Main(string[] args)
    {
        var mode = args[0];
        ServiceProvider serviceProvider = await ConfigureServices();
        EventProducer eventProducer = serviceProvider.GetRequiredService<EventProducer>();
        PostCreatedEvent postCreatedEvent = CreatePostCreatedEvent();
        await eventProducer.ProduceAsync("EventTopic", postCreatedEvent);

        switch (mode)
        {
            case "produce":
                ProducerConfig configProducer = serviceProvider.GetRequiredService<ProducerConfig>();
                Produce("Test", configProducer);
                break;
            case "consume":
                ConsumerConfig configConsumer = serviceProvider.GetRequiredService<ConsumerConfig>();
                Consume("Test", configConsumer);
                break;
            default:
                break;
        }
    }

    static void Produce(string topic, ClientConfig config)
    {
        using (var producer = new ProducerBuilder<string, string>(config).Build())
        {
            int numProduced = 0;
            int numMessages = 10;
            for (int i = 0; i < numMessages; ++i)
            {
                var key = "alice";
                var val = JObject.FromObject(new { count = i }).ToString(Formatting.None);

                Console.WriteLine($"Producing record: {key} {val}");

                producer.Produce(topic, new Message<string, string> { Key = key, Value = val },
                    (deliveryReport) =>
                    {
                        if (deliveryReport.Error.Code != ErrorCode.NoError)
                        {
                            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else
                        {
                            Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
                            numProduced += 1;
                        }
                    });
            }

            producer.Flush(TimeSpan.FromSeconds(10));

            Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
        }
    }

    static void Consume(string topic, ConsumerConfig config)
    {
        config.GroupId = "consumer-group-1";
        config.AutoOffsetReset = AutoOffsetReset.Earliest;
        //config.EnableAutoCommit = false;

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true; // evitar que o processo termine.
            cts.Cancel();
        };

        using (var consumer = new ConsumerBuilder<string, string>(config).Build())
        {
            consumer.Subscribe(topic);
            var totalCount = 0;
            try
            {
                while (true)
                {
                    var cr = consumer.Consume(cts.Token);
                    totalCount += JObject.Parse(cr.Message.Value).Value<int>("count");
                    Console.WriteLine($"Consumed record with key {cr.Message.Key} and value {cr.Message.Value}, and updated total count to {totalCount}");
                }
            }
            catch (OperationCanceledException)
            {
                // Ctrl-C pressionado
            }
            finally
            {
                consumer.Close();
            }
        }
    }

    static async Task<ServiceProvider> ConfigureServices()
    {
        ProducerConfig configProducer = await ConfigFiles.LoadConfig<ProducerConfig>("..\\config\\kafkaproducer.config");
        ConsumerConfig configConsumer = await ConfigFiles.LoadConfig<ConsumerConfig>("..\\config\\kafkaconsumer.config");

        var services = new ServiceCollection();
        services.AddSingleton<ProducerConfig>(configProducer);
        services.AddSingleton<ConsumerConfig>(configConsumer);

        services.AddScoped<EventProducer>();

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }

    static PostCreatedEvent CreatePostCreatedEvent()
    {
        PostCreatedEvent postCreatedEvent = new();
        postCreatedEvent.Id = new Guid();
        postCreatedEvent.Author = "Robert C. Martin";
        postCreatedEvent.DatePosted = DateTime.Today;
        postCreatedEvent.Message = "Truth can only be found in one place: the code.";
        postCreatedEvent.Type = postCreatedEvent.GetType().FullName;
        postCreatedEvent.Version = 0;
        return postCreatedEvent;
    }
}





