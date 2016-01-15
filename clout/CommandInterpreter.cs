// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandInterpreter.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the CommandInterpreter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;

namespace Clout
{
    public class CommandInterpreter
    {
        private readonly IEngine engine = new Engine();

        public CommandResult ExecuteCommand(string inputString)
        {
            var input = inputString.TrimEnd().TrimStart().Split();
            
            // No Input
            if (input.Length == 0)
                return new CommandResult(false, "No Input");

            // Single command
            if (input.Length == 1 && input[0] == Command.clout)
                return this.engine.Clout();
            
            // Multiple entries but may contain follows
            if (input.Length > 1)
            {
                if (input[0] == Command.clout)
                {
                    var name = inputString.Remove(0, Command.clout.Length + 1);
                    return this.engine.Clout(name);
                }

                if (input.Count(w => w == Command.follows) == 1)
                {
                    var delimiter = new[] { string.Format(" {0} ", Command.follows) };
                    var followArgs = inputString.Split(delimiter, StringSplitOptions.None);

                    return this.engine.Link(followArgs[0], followArgs[1]);
                }
            }

            return new CommandResult(false, string.Format("Error: unknown command <{0}>", inputString));
        }
    }
}
