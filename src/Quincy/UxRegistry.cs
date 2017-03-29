using MaterialDesignThemes.Wpf;
using Quincy.Progress;
using StructureMap;

namespace Quincy
{
    public class UxRegistry : Registry
    {
        public UxRegistry()
        {
            For<IDialogManager>().Use<DialogManager>().Singleton();
            For<IProgressReporter>().Use<ProgressReporter>().Singleton();

            For<ISnackbarMessageQueue>()
                .Use<SnackbarMessageQueue>()
                .SelectConstructor(() => new SnackbarMessageQueue())
                .Singleton();
        }
    }
}