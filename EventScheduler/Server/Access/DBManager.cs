using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Access
{
    class DBManager
    {
        #region Instance
        private static DBManager _instance;
        private static readonly object syncLock = new object();
        private DBManager() { }
        public static DBManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(syncLock)
                    {
                        if(_instance == null)
                        {
                            _instance = new DBManager();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        #region Person
        public bool AddPerson(Person person)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.Username.Equals(person.Username));
                if(found)
                {
                    return false;
                }

                dbContext.People.Add(person);
                dbContext.SaveChanges();
                return true;
            }
        }

        public bool ModifyPerson(Person person)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.Username.Equals(person.Username));
                if (found)
                {
                    Person foundPerson = dbContext.People.SingleOrDefault(p => p.Username.Equals(person.Username));
                    dbContext.People.Attach(foundPerson);

                    foundPerson.Username = person.Username;
                    foundPerson.Password = person.Password;
                    foundPerson.FirstName = person.FirstName;
                    foundPerson.LastName = person.LastName;
                    foundPerson.Role = person.Role;

                    dbContext.SaveChanges();
                    return true;
                }
                
                return false;
            }
        }

        public Person GetSinglePeson(String username)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.People.FirstOrDefault(p => p.Username.Equals(username));
            }
        }

        public Dictionary<String, Person> GetAllPeople()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Dictionary<String, Person> people = dbContext.People.ToDictionary(p => p.Username, p => p);
                if (people == null)
                {
                    people = new Dictionary<String, Person>();
                }

                return people;
            }
        }
        #endregion

        #region Event
        public bool AddEvent(Event newEvent)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Events.Any(e => e.EventID.Equals(newEvent.EventID));
                if (found)
                {
                    return false;
                }

                dbContext.Events.Add(newEvent);
                dbContext.SaveChanges();
                return true;
            }
        }

        public Event GetSingleEvent(String id)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.Events.FirstOrDefault(e => e.EventID.Equals(id));
            }
        }

        public Dictionary<Int32, Event> GetAllEvents()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Dictionary<Int32, Event> events = dbContext.Events.ToDictionary(e => e.EventID, e => e);
                if (events == null)
                {
                    events = new Dictionary<Int32, Event>();
                }

                return events;
            }
        }
        #endregion

    }
}
