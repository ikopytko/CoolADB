using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolADB.Sample
{
    class Program
    {
        private enum ConsoleCommands
        {
            Pass,
            Exit,
            KillServer,
            StartServer,
            ConnectedDevices,
        }

        private CoolADB.ADBClient _client;

        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            InitClient();
            ConsoleCommands command;

            Console.WriteLine("Select command:");

            do
            {
                ShowCommands();
                command = ReadCommand();
                ProcessCommand(command);
            }
            while (command != ConsoleCommands.Exit);
        }
        
        private void InitClient()
        {
            _client = new ADBClient();

            // assuming that adb.exe is in Debug directory
            _client.AdbPath = null;
        }

        #region Console processing 

        private static void Pause()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        private void ShowCommands()
        {
            var commands = (ConsoleCommands[])Enum.GetValues(typeof(ConsoleCommands));
            var count = commands.Length;

            for (int i = 1; i < count; i++)
            {
                Console.WriteLine($"{(int)commands[i]} {commands[i].ToString()}");
            }

            Console.Write("Command: ");
        }

        private ConsoleCommands ReadCommand()
        {
            var input = Console.ReadLine();
            int val;

            bool result = Int32.TryParse(input, out val);
            if (!result)
            {
                return ConsoleCommands.Pass;
            }

            if (val <= 0 || val > Enum.GetNames(typeof(ConsoleCommands)).Length)
                return ConsoleCommands.Pass;

            return (ConsoleCommands) val;
        }

        private void ProcessCommand(ConsoleCommands command)
        {
            Console.Clear();

            switch (command)
            {
                case ConsoleCommands.StartServer:
                    StartServer();
                    break;

                case ConsoleCommands.KillServer:
                    KillServer();
                    break;

                case ConsoleCommands.ConnectedDevices:
                    PrintConnectedDevices();
                    break;

                case ConsoleCommands.Pass:
                    Console.WriteLine("Wrong command");
                    break;

                case ConsoleCommands.Exit:
                    return;
            }

            Console.WriteLine();
        }

        #endregion

        private void StartServer()
        {
            _client.StartServer();
        }

        private void KillServer()
        {
            _client.KillServer();
        }

        private void PrintConnectedDevices()
        {
            var devices = _client.Devices();

            foreach (var device in devices)
            {
                Console.WriteLine(device);
            }

            Pause();
        }
    }
}
