using System;

namespace Quincy.Progress
{
    public interface IProgressReporter : IObservable<ProgressState>
    {
        void StartProgress();
        void StopProgress();
        void StartProgress(int maximum);
        void UpdateProgress(int current, int maximum);
    }
}