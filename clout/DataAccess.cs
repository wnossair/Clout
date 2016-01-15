// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAccess.cs" company="None">
//   None
// </copyright>
// <summary>
//   Defines the DataAccess type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Clout
{
    public static class DataAccess
    {
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

        public static IEnumerable<Person> GetAllPeople()
        {
            using (var db = new CloutContext())
            {
                return (from person in db.Person select person).ToList();
            }
        }

        public static IEnumerable<Person> GetFollowers(Person leader)
        {
            using (var db = new CloutContext())
            {
                return (from follower in db.Following
                        join person in db.Person on follower.PersonId equals person.Id
                        where follower.FollowingId == leader.Id
                        select person).ToList();
            }
        }
    }
}
