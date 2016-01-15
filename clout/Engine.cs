// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Engine.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the Engine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Clout
{
    public class Engine : IEngine
    {
        public const string FollowsSuccessMessage = "OK!";

        #region IEngine
        /// <summary>
        /// Adds a link between a follower and a leader
        /// A follower can only follow at most one person
        /// Responds to the command: [follower name] follows [leader name]
        /// </summary>
        /// <param name="followerName">The follower's name</param>
        /// <param name="leaderName">The leader's name</param>
        /// <returns>Whether it succeeded and the message to display</returns>
        public CommandResult Link(string followerName, string leaderName)
        {
            var follower = DataAccess.GetOrAddPerson(followerName);
            var leader = DataAccess.GetOrAddPerson(leaderName);

            DataAccess.AddOrUpdateLink(follower, leader);

            return new CommandResult(true, FollowsSuccessMessage);
        }

        /// <summary>
        /// Calculates the clout for all users in the database
        /// Responds to the command: clout
        /// </summary>
        /// <returns>Whether it succeeded and the results</returns>
        public CommandResult Clout()
        {
            var results = new ConcurrentDictionary<string, int>();
            IEnumerable<string> people = DataAccess.GetAllPeople();

            Parallel.ForEach(
                people,
                person => results.AddOrUpdate(
                    person,
                    GetClout(person),
                    (key, oldValue) => oldValue));

            return new CommandResult(
                true,
                string.Join(
                    Environment.NewLine,
                    results
                        .OrderByDescending(c => c.Value)
                        .Select(c => this.FormatCloutMessage(c.Key, c.Value))));
        }

        /// <summary>
        /// Calculates the clout for one user in the database
        /// Responds to the command: clout [person name]
        /// </summary>
        /// <param name="name">The person's name</param>
        /// <returns>Whether it succeeded and the results</returns>
        public CommandResult Clout(string name)
        {
            int clout = GetClout(name);
            var message = this.FormatCloutMessage(name, clout);

            return new CommandResult(true, message);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the clout of person
        /// This is a recursive function which traverses the tree up to it's roots
        /// </summary>
        /// <param name="current">The current person's name for which clout is being calculated</param>
        /// <param name="processed">
        /// A list containing the persons for which clout has been processed
        /// This is needed in order to escape loops in the graph
        /// Example: A follows B, B follows A - the recursion will continue infinitely. 
        /// By keeping track of which nodes we already processed we avoid the loop and potential double processing
        /// </param>
        /// <returns>The number of followers for the users</returns>
        private static int GetClout(string current, ConcurrentBag<string> processed = null)
        {
            if (processed == null) processed = new ConcurrentBag<string>(); // Not created yet? Create it
            if (processed.Contains(current)) return 0; // Already processed? break the recursion without affecting the calculation

            processed.Add(current);

            var leader = DataAccess.GetPerson(current);
            if (leader == null) throw new Exception(string.Format("Person {0} doesn't exist", current));

            var followers = DataAccess.GetFollowers(leader);

            // For each follower return the sum of 1 (for the node) and the clout of thier followers
            return followers.Sum(follower => 1 + GetClout(follower, processed));
        }


        /// <summary>
        /// Format the clout message
        /// </summary>
        /// <param name="name">Name of person</param>
        /// <param name="clout">Clout of person</param>
        /// <returns>Formatted string</returns>
        private string FormatCloutMessage(string name, int clout)
        {
            string message;
            switch (clout)
            {
                case 0:
                    message = string.Format("{0} has no followers", name);
                    break;
                case 1:
                    message = string.Format("{0} has 1 follower", name);
                    break;
                default:
                    message = string.Format("{0} has {1} followers", name, clout);
                    break;
            }

            return message;
        }
        
        #endregion
    }
}
