using System;
using System.Text;
using System.Threading.Tasks;

namespace Clout
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Controller
    {
        private IEngine engine = new Engine();

        public CommandResult InterpretCommand(string inputString)
        {
            var input = inputString.TrimEnd().TrimStart().Split();
            
            // No Input
            if (input.Length == 0)
                return new CommandResult(false, "No Input");

            // Single command
            if (input.Length == 1 && input[0] == Command.clout)
                return this.engine.Calculate();
            
            // Multiple entries but may contain follows
            if (input.Length > 1)
            {
                if (input[0] == Command.clout)
                {
                    var name = inputString.Remove(0, Command.clout.Length + 1);
                    return this.engine.Calculate(name);
                }

                if (input.Count(w => w == Command.follows) == 1)
                {
                    var delimiter = new[] { string.Format(" {0} ", Command.follows) };
                    var followArgs = inputString.Split(delimiter, StringSplitOptions.None);

                    return this.engine.AddLink(followArgs[0], followArgs[1]);
                }
            }

            return new CommandResult(false, string.Format("Error: unknown command <{0}>", inputString));
        }
    }

    public static class Command
    {
        public const string follows = @"follows";
        
        public const string clout = @"clout";

        public static List<string> Commands;

        static Command()
        {
            Commands = typeof(Command).GetFields()
                                      .Select(f => f.GetValue(null) as string)
                                      .ToList();
        }
    }

    public class CommandResult
    {
        public CommandResult(bool success, string content)
        {
            this.Success = success;
            this.Content = content;
        }

        public bool Success { get; private set; }

        public string Content { get; private set; }
    }
}
