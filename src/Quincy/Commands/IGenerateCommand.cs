using System.Windows.Input;

namespace Quincy.Commands
{
    public interface IGenerateCommand
    {
        ICommand Generate();
    }
}