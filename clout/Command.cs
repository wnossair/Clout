// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Command.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the Command type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Clout
{
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
}