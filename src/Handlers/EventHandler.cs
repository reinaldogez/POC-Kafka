using System.Text;
using PocKafka.Events;

namespace PocKafka.Handlers;

public class EventHandler : IEventHandler
{
    public void On(PostCreatedEvent @event)
    {
        StringBuilder sb = new();
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
