// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandResult.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the CommandResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clout
{
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