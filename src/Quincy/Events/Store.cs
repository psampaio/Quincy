using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReactiveUI;

namespace Quincy.Events
{
    public class Store : IStore
    {
        private readonly MultiInstanceFactory multiInstanceFactory;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IList<IStoreState> storeStates = new List<IStoreState>();

        public Store(IEnumerable<IStoreStateProvider> initialStateProviders, MultiInstanceFactory multiInstanceFactory)
        {
            this.multiInstanceFactory = multiInstanceFactory;

            foreach (var initialStateProvider in initialStateProviders)
                storeStates.Add(initialStateProvider.GetState());
        }

        public async Task Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var tasks = new List<Task>();
                foreach (var storeState in storeStates)
                {
                    var task = GetHandlerTasksFor(@event, storeState.StateAsObject);
                    await task.ContinueWith(_ => storeState.UpdateSubscribers());
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public StoreState<TState> GetStoreState<TState>() where TState : ReactiveObject
        {
            return storeStates.OfType<StoreState<TState>>().Single();
        }

        private Task GetHandlerTasksFor<TEvent>(TEvent @event, ReactiveObject state)
            where TEvent : IEvent
        {
            var instance = (EventHandlerWrapper)Activator.CreateInstance(typeof(EventHandlerWrapperImpl<,>).MakeGenericType(@event.GetType(), state.GetType()));
            return instance.Handle(@event, state, multiInstanceFactory);
        }
    }
}