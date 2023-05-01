using System.Text;
using PocKafka.Events;

namespace PocKafka;

public class EventHandler
{
    public async Task On(PostCreatedEvent @event)
    {
        StringBuilder sb = new();
        sb.AppendLine("New message received by EventConsumer");
        sb.AppendLine($"EventType: {@event.Type}");
        sb.AppendLine($"PostId: {@event.Id}");
        sb.AppendLine($"Author: {@event.Author}");
        sb.AppendLine($"DatePosted: {@event.DatePosted}");
        sb.AppendLine($"Message: {@event.Message}");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(sb.ToString());
        Console.ResetColor();
    }
}
