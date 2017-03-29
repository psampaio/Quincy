using System;
using System.Reactive.Subjects;

namespace Quincy.Progress
{
    public class ProgressReporter : IProgressReporter
    {
        private readonly ProgressState state;
        private readonly ReplaySubject<ProgressState> stateSubject = new ReplaySubject<ProgressState>(1);

        public ProgressReporter()
        {
            state = new ProgressState();
        }

        public IDisposable Subscribe(IObserver<ProgressState> observer)
        {
            return stateSubject.Subscribe(observer);
        }

        public void StartProgress()
        {
            state.Enabled = true;
            state.Indeterminate = true;

            stateSubject.OnNext(state);
        }

        public void StopProgress()
        {
            state.Enabled = false;

            stateSubject.OnNext(state);
        }

        public void StartProgress(int maximum)
        {
            state.Enabled = true;
            state.Indeterminate = false;
            state.CurrentProgress = 0;
            state.MaximumProgress = maximum;

            stateSubject.OnNext(state);
        }

        public void UpdateProgress(int current, int maximum)
        {
            state.Indeterminate = false;
            state.CurrentProgress = current;
            state.MaximumProgress = maximum;

            stateSubject.OnNext(state);
        }
    }
}