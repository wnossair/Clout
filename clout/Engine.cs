// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Defines the Engine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clout
{
    using System.Collections.Concurrent;
    using System.Data.Entity.Migrations;

    public class Engine : IEngine
    {
        public CommandResult AddLink(string followerName, string leaderName)
        {
            const string SuccessMessage = "OK!";
            try
            {
                using (var db = new CloutContext())
                {
                    var follower = GetOrAddPerson(followerName, db);
                    var leader = GetOrAddPerson(leaderName, db);

                    db.Following.AddOrUpdate(new Following { PersonId = follower.Id, FollowingId = leader.Id });
                    db.SaveChanges();
                }

                return new CommandResult(true, SuccessMessage);
            }
            catch (Exception e)
            {
                return new CommandResult(false, e.ToString());
            }
        }

        private static Person GetOrAddPerson(string name, CloutContext db)
        {
            var person = db.Person.SingleOrDefault(p => p.Name == name);
            if (person == null)
                db.Person.Add(new Person { Name = name });

            db.SaveChanges();
            return person ?? db.Person.SingleOrDefault(p => p.Name == name);
        }

        private static bool PersonExists(string name, CloutContext db)
        {
            return db.Person.SingleOrDefault(p => p.Name == name) == null;
        }
        
        public int GetClout(string current, List<string> done = null)
        {
            if (done == null) done = new List<string>();
            if (done.Contains(current)) return 0;

            done.Add(current);

            using (var db = new CloutContext())
            {
                var leader = db.Person.SingleOrDefault(p => p.Name == current);
                var followers = (from follower in db.Following
                                 join person in db.Person on follower.PersonId equals person.Id
                                 where follower.FollowingId == leader.Id
                                 select person.Name).ToList();

                return followers.Sum(follower => 1 + this.GetClout(follower, done));
            }
        }

        public CommandResult Calculate()
        {
            var results = new ConcurrentDictionary<string, int>();
            using (var db = new CloutContext())
            {
                IEnumerable<string> people = from person in db.Person select person.Name;
                Parallel.ForEach(people, 
                    person => results.AddOrUpdate(
                        person,
                        this.GetClout(person), 
                        (key, oldValue) => oldValue));
            }

            return new CommandResult(
                true,
                string.Join(
                    Environment.NewLine, 
                    results.OrderByDescending(c => c.Value).Select(c => GetMessage(c.Key, c.Value))));
        }

        public CommandResult Calculate(string name)
        {
            int clout = this.GetClout(name);
            var message = GetMessage(name, clout);

            return new CommandResult(true, message);
        }

        private static string GetMessage(string name, int clout)
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
    }
}
