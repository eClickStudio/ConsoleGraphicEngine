using System;

namespace Commands
{
    public class Command : ICommand
    {
        public string description { get; }
        private Action _method;

        public Command(string description, Action method)
        {
            this.description = description;
            _method = method;
        }

        public void Execute()
        {
            _method?.Invoke();
        }
    }
}
