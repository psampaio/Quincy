using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Quincy.Commands
{
    public delegate IGenerateCommand CommandGeneratorFactory(Type serviceType);

    public class CommandManager : ICommandManager
    {
        private readonly IDictionary<Type, ICommand> commandCache;
        private readonly CommandGeneratorFactory commandGeneratorFactory;

        public CommandManager(CommandGeneratorFactory commandGeneratorFactory)
        {
            this.commandGeneratorFactory = commandGeneratorFactory;

            commandCache = new Dictionary<Type, ICommand>();
        }

        public ICommand Get<TCommand>() where TCommand : IGenerateCommand
        {
            ICommand command;
            if (commandCache.TryGetValue(typeof(TCommand), out command))
                return command;

            var commandGenerator = commandGeneratorFactory(typeof(TCommand));
            command = commandGenerator.Generate();
            commandCache.Add(typeof(TCommand), command);

            return command;
        }
    }
}