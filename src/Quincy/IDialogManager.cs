using System.Threading.Tasks;
using Caliburn.Micro;

namespace Quincy
{
    public interface IDialogManager
    {
        Task<TReturn> ShowDialog<TViewModel, TReturn>(TViewModel rootModel)
            where TViewModel : class, IScreen;
    }
}