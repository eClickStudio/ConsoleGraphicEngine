using System;
using System.Collections.Generic;

namespace Commands
{
    public class CommandManager : ICommandManager
    {
        private Dictionary<string, ICommand> _commands { get; }
        private ICommand _helpCommand { get; }

        public CommandManager()
        {
            _helpCommand = new Command("Write all admissible commands with discriptions to the console.", ShowAllCommands);

            _commands = new Dictionary<string, ICommand>()
            {
                { "help", _helpCommand },
            };
        }


        public void Help()
        {
            _helpCommand.Execute();
        }

        public void HandleCommand(string commandKey)
        {
            if (_commands.ContainsKey(commandKey))
            {
                _commands[commandKey].Execute();
            }
            else
            {
                throw new ArgumentException($"Command you want to execute is not exist! Command key = {commandKey}");
            }
        }

        private void ShowAllCommands()
        {
            foreach (KeyValuePair<string, ICommand> pair in _commands)
            {
                Console.WriteLine($"{pair.Key} - {pair.Value.description}");
            }
        }


        public bool ContainCommand(in string commandKey)
        {
            return _commands.ContainsKey(commandKey);
        }

        private bool ContainCommandsArray(in string[] commandKeys)
        {
            foreach (string key in commandKeys)
            {
                if (!ContainCommand(key))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainCommands(in string[] commandKeys)
        {
            return ContainCommandsArray(commandKeys);
        }

        public bool ContainCommands(params string[] commandKeys)
        {
            return ContainCommandsArray(commandKeys);
        }

        public bool ContainCommand(in ICommand command)
        {
            return _commands.ContainsValue(command);
        }

        private bool ContainCommandsArray(in ICommand[] commandKeys)
        {
            foreach (ICommand key in commandKeys)
            {
                if (!ContainCommand(key))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainCommands(in ICommand[] commands)
        {
            return ContainCommandsArray(commands);
        }

        public bool ContainCommands(params ICommand[] commands)
        {
            return ContainCommandsArray(commands);
        }


        public void AddCommand(in string key, in ICommand command)
        {
            _commands.Add(key, command);
        }

        public void AddCommands(in IReadOnlyDictionary<string, ICommand> commands)
        {
            foreach (KeyValuePair<string, ICommand> pair in commands)
            {
                _commands.Add(pair.Key, pair.Value);
            }
        }


        public void RemoveCommand(in string commandKey)
        {
            if (commandKey == "help")
            {
                throw new ArgumentException("You can not remove help command!");
            }

            if (ContainCommand(commandKey))
            {
                _commands.Remove(commandKey);
            }
        }

        private void RemoveCommandsArray(in string[] commandKeys)
        {
            foreach (string key in commandKeys)
            {
                RemoveCommand(key);
            }
        }

        public void RemoveCommands(in string[] commandKeys)
        {
            RemoveCommandsArray(commandKeys);
        }

        public void RemoveCommands(params string[] commandKeys)
        {
            RemoveCommandsArray(commandKeys);
        }
    }
}
