using System.Reflection;
using System.Text.Json;
using Confluent.Kafka;
using PocKafka.Events;
using PocKafka.Handlers;
using PocKafka.Utils;

namespace PocKafka;

public class EventConsumer
{
    private readonly ConsumerConfig _config;
    private readonly IEventHandler _eventHandler;

    public EventConsumer(ConsumerConfig config, IEventHandler eventHandler)
    {
        _config = config;
        _eventHandler = eventHandler;
    }

    public void Consume(string topic, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
                    .SetKeyDeserializer(Deserializers.Utf8)
                    .SetValueDeserializer(Deserializers.Utf8)
                    .Build();

        consumer.Subscribe(topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Consumer Started");
                Console.ResetColor();
                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(3));

                if (consumeResult?.Message == null) continue;
                
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("New message received by EventConsumer");
                Console.WriteLine($"MessakeKey: {consumeResult.Message.Key}");
                Console.ResetColor();

                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
                MethodInfo handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

                if (handlerMethod == null)
                {
                    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
                }

                handlerMethod.Invoke(_eventHandler, new object[] { @event });

                consumer.Commit(consumeResult);
            }
        }
        catch (OperationCanceledException oce)
        {
            Console.WriteLine($"OperationCanceledException on EventConsumer: {oce.Message}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception on EventConsumer: {ex.Message}");
        }
        finally
        {
            consumer.Close();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Consumer Closed");
            Console.ResetColor();
        }
    }
}
