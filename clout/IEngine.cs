// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEngine.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the IEngine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clout
{
    public interface IEngine
    {
        /// <summary>
        /// Adds a link between a follower and a leader
        /// A follower can only follow at most one person
        /// Responds to the command: [follower name] follows [leader name]
        /// </summary>
        /// <param name="follower">The follower's name</param>
        /// <param name="leader">The leader's name</param>
        /// <returns>Whether it succeeded and the message to display</returns>
        CommandResult Link(string follower, string leader);

        /// <summary>
        /// Calculates the clout for all users in the database
        /// Responds to the command: clout
        /// </summary>
        /// <returns>Whether it succeeded and the results</returns>
        CommandResult Clout();

        /// <summary>
        /// Calculates the clout for one user in the database
        /// Responds to the command: clout [person name]
        /// </summary>
        /// <param name="name">The person's name</param>
        /// <returns>Whether it succeeded and the results</returns>
        CommandResult Clout(string name);
    }
}