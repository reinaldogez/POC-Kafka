using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PocKafka;
using PocKafka.Events;
using PocKafka.Handlers;
using PocKafka.Utils;


ServiceProvider serviceProvider = await ConfigureServices();

try
{
    await Go();
}
catch (Exception e)
{
    Console.WriteLine("Error: {0}", e);
}
finally
{
    Console.WriteLine("End of POC-Kafka, press any key to exit.");
    Console.ReadLine();
}

async Task Go()
{
    CancellationTokenSource cts = new CancellationTokenSource();
    Console.CancelKeyPress += (sender, e) =>
    {
        e.Cancel = true;
        Console.WriteLine("CancelKeyPress event triggered");
        cts.Cancel();
        Environment.Exit(0);
    };
    EventConsumer eventConsumer = serviceProvider.GetRequiredService<EventConsumer>();

#if DEBUG

    Console.WriteLine("Debug mode is on.");

    // Start the eventConsumer in a separate Task
    Task consumeTask = Task.Run(() => eventConsumer.Consume("EventTopic", cts.Token));

    while (!cts.Token.IsCancellationRequested)
    {
        await Task.Delay(1000);
    }
    Environment.Exit(0);

#endif

    Console.WriteLine("Debug mode is off.");
    while (true)
    {
        PrintPrompt();
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.D0:
                EventProducer eventProducer = serviceProvider.GetRequiredService<EventProducer>();
                PostCreatedEvent postCreatedEvent = CreatePostCreatedEvent();
                await eventProducer.ProduceAsync("EventTopic", postCreatedEvent);
                break;
            case ConsoleKey.D1:
                Task startConsumerTask = Task.Run(() => eventConsumer.Consume("EventTopic", cts.Token));
                break;
            case ConsoleKey.D2:
                break;
            case ConsoleKey.D3:
                break;
            case ConsoleKey.D4:
                break;
            case ConsoleKey.D5:

                break;
            case ConsoleKey.D6:

                break;
            case ConsoleKey.D7:

                break;
            case ConsoleKey.D8:

                break;
            case ConsoleKey.Escape:
                Console.WriteLine("Exiting...");
                return;

            default:
                Console.WriteLine("Select choice");
                break;
        }
    }
}

void PrintPrompt()
{
    Console.WriteLine("0 - Scenario 0: Produce PostCreatedEvent");
    Console.WriteLine("1 - Scenario 1: Start EventConsumer");
}


static async Task<ServiceProvider> ConfigureServices()
{
    ProducerConfig configProducer = await ConfigFiles.LoadConfig<ProducerConfig>("..\\config\\kafkaproducer.config");
    ConsumerConfig configConsumer = await ConfigFiles.LoadConfig<ConsumerConfig>("..\\config\\kafkaconsumer.config");

    var services = new ServiceCollection();
    services.AddSingleton<ProducerConfig>(configProducer);
    services.AddSingleton<ConsumerConfig>(configConsumer);

    services.AddScoped<EventProducer>();
    services.AddScoped<EventConsumer>();
    services.AddScoped<IEventHandler, PocKafka.Handlers.EventHandler>();

    ServiceProvider serviceProvider = services.BuildServiceProvider();
    return serviceProvider;
}

static PostCreatedEvent CreatePostCreatedEvent()
{
    PostCreatedEvent postCreatedEvent = new();
    postCreatedEvent.Id = Guid.NewGuid();
    postCreatedEvent.Author = "Robert C. Martin";
    postCreatedEvent.DatePosted = DateTime.Now;
    postCreatedEvent.Message = "Truth can only be found in one place: the code.";
    postCreatedEvent.Type = nameof(PostCreatedEvent);
    postCreatedEvent.Version = 0;
    return postCreatedEvent;
}