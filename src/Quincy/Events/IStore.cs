using System.Threading.Tasks;
using ReactiveUI;

namespace Quincy.Events
{
    public interface IStore
    {
        Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;
        StoreState<TState> GetStoreState<TState>() where TState : ReactiveObject;
    }
}