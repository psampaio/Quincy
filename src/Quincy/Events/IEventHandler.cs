using System.Threading.Tasks;
using ReactiveUI;

namespace Quincy.Events
{
    public interface IEventHandler<in TEvent, in TState>
        where TEvent : IEvent
        where TState : ReactiveObject
    {
        Task Handle(TEvent @event, TState state);
    }
}