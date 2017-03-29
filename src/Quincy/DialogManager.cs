using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;

namespace Quincy
{
    public class DialogManager : IDialogManager
    {
        private readonly Conductor<IScreen> conductor;

        public DialogManager()
        {
            conductor = new Conductor<IScreen>();
            ((IActivate)conductor).Activate();
        }

        public async Task<TReturn> ShowDialog<TViewModel, TReturn>(TViewModel rootModel, string dialogIdentifier)
            where TViewModel : class, IScreen
        {
            var viewType = ViewLocator.LocateTypeForModelType(typeof(TViewModel), null, null);
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            ViewModelBinder.Bind(rootModel, view, null);
            view.DataContext = rootModel;

            conductor.ActivateItem(rootModel);
            var returnValue = await DialogHost.Show(view, dialogIdentifier, OnDialogClosing);

            if (returnValue == null)
                return default(TReturn);

            return (TReturn)returnValue;
        }

        private void OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            var screen = conductor.ActiveItem;
            screen.TryClose();

            if (conductor.ActiveItem != null)
                eventArgs.Cancel();
        }
    }
}