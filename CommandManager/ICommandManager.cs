using System.Collections.Generic;

namespace Commands
{
    public interface ICommandManager
    {
        /// <summary>
        /// Handle help command
        /// </summary>
        void Help();

        /// <summary>
        /// Handle the command
        /// </summary>
        /// <param name="command">Command you want to handle</param>
        void HandleCommand(string command);


        /// <summary>
        /// Is there command?
        /// </summary>
        /// <param name="commandKey">Key phrase to invoke the command</param>
        /// <returns></returns>
        bool ContainCommand(in string commandKey);

        /// <summary>
        /// Is there commands?
        /// </summary>
        /// <param name="commandKeys">Key phrases array to invoke the commands</param>
        /// <returns></returns>
        bool ContainCommands(in string[] commandKeys);

        /// <summary>
        /// Is there commands?
        /// </summary>
        /// <param name="commandKeys">Key phrases to invoke the commands</param>
        /// <returns></returns>
        bool ContainCommands(params string[] commandKeys);


        /// <summary>
        /// Is there command?
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        bool ContainCommand(in ICommand command);

        /// <summary>
        /// Is there commands?
        /// </summary>
        /// <param name="commands">Commands array</param>
        /// <returns></returns>
        bool ContainCommands(in ICommand[] commands);

        /// <summary>
        /// Is there commands?
        /// </summary>
        /// <param name="commands">Commands</param>
        /// <returns></returns>
        bool ContainCommands(params ICommand[] commands);


        /// <summary>
        /// Adds command
        /// </summary>
        /// <param name="key">Key phrases to invoke the command</param>
        /// <param name="command">Command you want to add</param>
        void AddCommand(in string key, in ICommand command);

        /// <summary>
        /// Adds commands
        /// </summary>
        /// <param name="commands">Commands dictionary you want to add</param>
        void AddCommands(in IReadOnlyDictionary<string, ICommand> commands);


        /// <summary>
        /// Removes command
        /// </summary>
        /// <param name="commandKey">Key phrase to invoke the command</param>
        void RemoveCommand(in string commandKey);

        /// <summary>
        /// Removes commands
        /// </summary>
        /// <param name="commandKeys">Key phrases array to invoke the commands</param>
        void RemoveCommands(in string[] commandKeys);

        /// <summary>
        /// Removes commands
        /// </summary>
        /// <param name="commandKeys">Key phrases to invoke the commands</param>
        void RemoveCommands(params string[] commandKeys);
    }
}
