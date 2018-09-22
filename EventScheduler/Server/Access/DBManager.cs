using Common.IAccess;
using Common.IModels;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Access
{
    class DbManager : IPersonDbManager, IPersonWithAccountDbManager, IEventDbManager
    {
        #region Instance
        private static DbManager _instance;
        private static readonly object syncLock = new object();
        private DbManager() { }
        public static DbManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(syncLock)
                    {
                        if(_instance == null)
                        {
                            _instance = new DbManager();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        #region Person
        public bool AddPerson(Person personToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.PersonID.Equals(personToAdd.PersonID));
                if (found)
                {
                    return false;
                }

                dbContext.People.Add(personToAdd);
                dbContext.SaveChanges();
                return true;
            }
        }

        public bool ModifyPerson(Person personToModify)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.PersonID.Equals(personToModify.PersonID));
                if (found)
                {
                    Person foundPerson = dbContext.People.SingleOrDefault(p => p.PersonID.Equals(personToModify.PersonID));
                    dbContext.People.Attach(foundPerson);

                    foundPerson.PersonID = personToModify.PersonID;
                    foundPerson.FirstName = personToModify.FirstName;
                    foundPerson.LastName = personToModify.LastName;
                    personToModify.ScheduledEvents.ForEach(e => foundPerson.ScheduleParticipationInEvent(e));

                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool DeletePerson(Person personToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.PersonID.Equals(personToDelete.PersonID));
                if (found)
                {
                    Person foundPerson = dbContext.People.SingleOrDefault(p => p.PersonID.Equals(personToDelete.PersonID));
                    dbContext.People.Remove(foundPerson);
                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public Person GetSinglePerson(int personId)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.People.FirstOrDefault(p => p.PersonID.Equals(personId));
            }
        }

        public Dictionary<int, Person> GetAllPeople()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Dictionary<int, Person> people = dbContext.People.ToDictionary(p => p.PersonID, p => p);
                if (people == null)
                {
                    people = new Dictionary<int, Person>();
                }

                return people;
            }
        }
        #endregion

        #region PersonWithAccount
        public bool AddPersonWithAccount(PersonWithAccount personWithAccountToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.PeopleWithAccounts.Any(p => p.PersonID.Equals(personWithAccountToAdd.PersonID)
                                                                   || p.Username.Equals(personWithAccountToAdd.Username));
                if (found)
                {
                    return false;
                }

                dbContext.PeopleWithAccounts.Add(personWithAccountToAdd);
                dbContext.SaveChanges();
                return true;
            }
        }

        public bool ModifyPersonWithAccount(PersonWithAccount personWithAccountToModify)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.PeopleWithAccounts.Any(p => p.PersonID.Equals(personWithAccountToModify.PersonID));
                if (found)
                {
                    PersonWithAccount foundPersonWithAccount = dbContext.PeopleWithAccounts.SingleOrDefault(p => p.PersonID.Equals(personWithAccountToModify.PersonID));
                    dbContext.PeopleWithAccounts.Attach(foundPersonWithAccount);

                    foundPersonWithAccount.PersonID = personWithAccountToModify.PersonID;
                    foundPersonWithAccount.Username = personWithAccountToModify.Username;
                    foundPersonWithAccount.Password = personWithAccountToModify.Password;
                    foundPersonWithAccount.Role = personWithAccountToModify.Role;
                    foundPersonWithAccount.FirstName = personWithAccountToModify.FirstName;
                    foundPersonWithAccount.LastName = personWithAccountToModify.LastName;
                    personWithAccountToModify.ScheduledEvents.ForEach(e => foundPersonWithAccount.ScheduleParticipationInEvent(e));

                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool DeletePersonWithAccount(PersonWithAccount personWithAccountToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.PeopleWithAccounts.Any(p => p.PersonID.Equals(personWithAccountToDelete.PersonID));
                if (found)
                {
                    PersonWithAccount foundPersonWithAccount = dbContext.PeopleWithAccounts.SingleOrDefault(p => p.PersonID.Equals(personWithAccountToDelete.PersonID));
                    dbContext.PeopleWithAccounts.Remove(foundPersonWithAccount);
                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public PersonWithAccount GetSinglePersonWithAccount(Int32 personWithAccountId)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.PeopleWithAccounts.FirstOrDefault(p => p.PersonID.Equals(personWithAccountId));
            }
        }

        public PersonWithAccount GetSinglePersonWithAccountByUsername(String personWithAccountUsername)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.PeopleWithAccounts.FirstOrDefault(p => p.Username.Equals(personWithAccountUsername));
            }
        }

        public Dictionary<int, PersonWithAccount> GetAllPeopleWithAccounts()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Dictionary<int, PersonWithAccount> peopleWithAccounts = dbContext.PeopleWithAccounts.ToDictionary(p => p.PersonID, p => p);
                if (peopleWithAccounts == null)
                {
                    peopleWithAccounts = new Dictionary<int, PersonWithAccount>();
                }

                return peopleWithAccounts;
            }
        }
        #endregion

        #region Event
        public bool AddEvent(Event eventToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Events.Any(e => e.EventID.Equals(eventToAdd.EventID));
                if (found)
                {
                    return false;
                }

                dbContext.Events.Add(eventToAdd);
                dbContext.SaveChanges();
                return true;
            }
        }

        public bool ModifyEvent(Event eventToModify)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Events.Any(p => p.EventID.Equals(eventToModify.EventID));
                if (found)
                {
                    Event foundEvent = dbContext.Events.SingleOrDefault(e => e.EventID.Equals(eventToModify.EventID));
                    dbContext.Events.Attach(foundEvent);

                    foundEvent.LastEditTimeStamp = eventToModify.LastEditTimeStamp;
                    foundEvent.ScheduledDateTimeBeging = eventToModify.ScheduledDateTimeBeging;
                    foundEvent.ScheduledDateTimeEnd = eventToModify.ScheduledDateTimeEnd;
                    foundEvent.Description = eventToModify.Description;
                    eventToModify.Participants.ForEach(p => foundEvent.AddParticipant(p));

                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool DeleteEvent(Event eventToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Events.Any(p => p.EventID.Equals(eventToDelete.EventID));
                if (found)
                {
                    Event foundEvent = dbContext.Events.SingleOrDefault(e => e.EventID.Equals(eventToDelete.EventID));
                    dbContext.Events.Remove(foundEvent);
                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public Event GetSingleEvent(Int32 eventId)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.Events.FirstOrDefault(e => e.EventID.Equals(eventId));
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
