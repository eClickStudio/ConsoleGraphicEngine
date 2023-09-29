namespace Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Command description
        /// </summary>
        string description { get; }

        /// <summary>
        /// Exucute linked method
        /// </summary>
        void Execute();
    }
}
