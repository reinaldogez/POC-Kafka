using PocKafka.Events;

namespace PocKafka.Handlers;
public interface IEventHandler
{
    void On(PostCreatedEvent @event);
}