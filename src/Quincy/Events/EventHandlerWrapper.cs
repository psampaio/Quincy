using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ReactiveUI;

namespace Quincy.Events
{
    internal abstract class EventHandlerWrapper
    {
        public abstract Task Handle(IEvent @event, ReactiveObject state, MultiInstanceFactory factory);
    }

    internal class EventHandlerWrapperImpl<TEvent, TState> : EventHandlerWrapper where TEvent : IEvent where TState : ReactiveObject
    {
        public override Task Handle(IEvent @event, ReactiveObject state, MultiInstanceFactory factory)
        {
            var tasks = GetTasks(@event, state, factory);
            return Task.WhenAll(tasks);
        }

        private IEnumerable<Task> GetTasks(IEvent @event, ReactiveObject state, MultiInstanceFactory factory)
        {
            var handlers = factory(typeof(IEventHandler<TEvent, TState>)).Cast<IEventHandler<TEvent, TState>>();
            foreach (var handler in handlers)
                yield return handler.Handle((TEvent)@event, (TState)state);
        }
    }

}