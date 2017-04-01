using System;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Quincy.Events
{
    public class StoreState<TState> : IStoreState, IObservable<TState>
        where TState : ReactiveObject
    {
        private readonly ReplaySubject<TState> stateSubject = new ReplaySubject<TState>(1);

        public StoreState(TState state)
        {
            State = state;
        }

        public TState State { get; }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return stateSubject.Subscribe(observer);
        }

        void IStoreState.UpdateSubscribers()
        {
            stateSubject.OnNext(State);
        }

        ReactiveObject IStoreState.StateAsObject => State;
    }
}