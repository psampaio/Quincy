using ReactiveUI;

namespace Quincy.Events
{
    public abstract class StoreStateProvider<TState> : IStoreStateProvider where TState : ReactiveObject
    {
        public IStoreState GetState()
        {
            var state = GetInitialState();

            return new StoreState<TState>(state);
        }

        protected abstract TState GetInitialState();
    }

}