using System.Threading.Tasks;
using PocKafka.Events;
using PocKafka.Handlers;

namespace PocKafka.Tests.Integration;

public class TestEventHandler : IEventHandler
{
    public PostCreatedEvent HandledEvent { get; private set; }
    public TaskCompletionSource<PostCreatedEvent> EventHandledCompletionSource { get; } = new TaskCompletionSource<PostCreatedEvent>();

    public void On(PostCreatedEvent @event)
    {
        HandledEvent = @event;
        EventHandledCompletionSource.SetResult(@event);
    }
}