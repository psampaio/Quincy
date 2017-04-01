using ReactiveUI;

namespace Quincy.Events
{
    public interface IStoreState
    {
        ReactiveObject StateAsObject { get; }

        void UpdateSubscribers();
    }
}