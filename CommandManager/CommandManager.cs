//using Commands.Abstract;
//using System;
//using System.Threading.Tasks;

//namespace Commands
//{
//    internal class CommandManager : CommandHandler, ICommandManager
//    { 
//        private Task _handlingTask;

//        public bool IsHandling { get; private set; }

//        public CommandManager(ConsoleColorSet colorSet) : base(colorSet)
//        {
//            AddCommand("stophandling", new Command("Stops commands handling", async () => await StopHandlingCommands()));
//        }

//        public Task StartHandlingCommands()
//        {
//            _handlingTask = Task.Run(() => { });
//        }

//        private async Task HandlingCommands()
//        {
//            while (IsHandling)
//            {
//                await Console.ReadLine();
//            }
//        }

//        public async Task StopHandlingCommands()
//        {
//            IsHandling = false;

//            await _handlingTask;
//        }
//    }
//}
