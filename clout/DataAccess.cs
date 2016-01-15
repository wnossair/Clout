using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clout
{
    using System.Data.Entity.Migrations;

    public static class DataAccess
    {
        /// <summary>
        /// Gets a person by name using the context of the database passed
        /// If the person is not in the database, it adds it first
        /// </summary>
        /// <param name="name">
        /// The name of the person
        /// </param>
        /// <returns>
        /// The Person from the database
        /// </returns>
        public static Person GetOrAddPerson(string name)
        {
            using (var db = new CloutContext())
            {
                var person = db.Person.SingleOrDefault(p => p.Name == name);
                if (person == null)
                {
                    db.Person.Add(new Person { Name = name });
                    db.SaveChanges();
                }

                return person ?? db.Person.SingleOrDefault(p => p.Name == name);
            }
        }

        public static void AddOrUpdateLink(Person follower, Person leader)
        {
            using (var db = new CloutContext())
            {
                db.Following.AddOrUpdate(new Following { PersonId = follower.Id, FollowingId = leader.Id });
                db.SaveChanges();
            }
        }

        public static Person GetPerson(string name)
        {
            using (var db = new CloutContext())
            {
                return db.Person.SingleOrDefault(p => p.Name == name);
            }
        }

        public static IEnumerable<string> GetAllPeople()
        {
            using (var db = new CloutContext())
            {
                return (from person in db.Person select person.Name).ToList();
            }
        }

        public static IEnumerable<string> GetFollowers(Person leader)
        {
            using (var db = new CloutContext())
            {
                return (from follower in db.Following
                        join person in db.Person on follower.PersonId equals person.Id
                        where follower.FollowingId == leader.Id
                        select person.Name).ToList();
            }
        }
    }
}
