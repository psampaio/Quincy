using System.Windows.Input;

namespace Quincy.Commands
{
    public interface ICommandManager
    {
        ICommand Get<TCommand>() where TCommand : IGenerateCommand;
    }
}