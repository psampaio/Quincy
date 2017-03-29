using ReactiveUI;

namespace Quincy.Progress
{
    public class ProgressState : ReactiveObject
    {
        private int currentProgress;
        private bool enabled;
        private bool indeterminate;
        private int maximumProgress;

        public bool Enabled
        {
            get { return enabled; }
            set { this.RaiseAndSetIfChanged(ref enabled, value); }
        }

        public bool Indeterminate
        {
            get { return indeterminate; }
            set { this.RaiseAndSetIfChanged(ref indeterminate, value); }
        }

        public int CurrentProgress
        {
            get { return currentProgress; }
            set { this.RaiseAndSetIfChanged(ref currentProgress, value); }
        }

        public int MaximumProgress
        {
            get { return maximumProgress; }
            set { this.RaiseAndSetIfChanged(ref maximumProgress, value); }
        }
    }
}