// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Clout
{
    public class Program
    {
        private const string ReadPrompt = "> ";
        private const string Exit = "exit";

        static void Main(string[] args)
        {
            var interpreter = new CommandInterpreter();

            while (true)
            {
                var consoleInput = ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;
                if (consoleInput == Exit) break;

                try
                {
                    var result = interpreter.ExecuteCommand(consoleInput);
                    WriteToConsole(result.Content);
                }
                catch (Exception ex)
                {
                    WriteToConsole(ex.ToString());
                }
            }
        }

        private static string ReadFromConsole(string promptMessage = "")
        {
            Console.Write(ReadPrompt + promptMessage);
            return Console.ReadLine();
        }

        private static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
                Console.WriteLine(message);
        }
    }
}
