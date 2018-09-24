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
    class DbManager : IPersonDbManager, IAccountDbManager, IEventDbManager
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
                bool found = dbContext.People.Any(p => p.JMBG.Equals(personToAdd.JMBG));
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
                bool found = dbContext.People.Any(p => p.JMBG.Equals(personToModify.JMBG));
                if (found)
                {
                    Person foundPerson = dbContext.People.SingleOrDefault(p => p.JMBG.Equals(personToModify.JMBG));
                    dbContext.People.Attach(foundPerson);

                    foundPerson.JMBG = personToModify.JMBG;
                    foundPerson.FirstName = personToModify.FirstName;
                    foundPerson.LastName = personToModify.LastName;
                    foundPerson.LastEditTimeStamp = DateTime.Now;
                    personToModify.ScheduledEvents.ForEach(e => foundPerson.ScheduleParticipationInEvent(e));

                    dbContext.SaveChanges();

                    //OVDE NEGDE NOTIFIKACIJA ZA POTENCIJALNI KONFLIKT

                    return true;
                }

                return false;
            }
        }

        public bool DeletePerson(Person personToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.JMBG.Equals(personToDelete.JMBG));
                if (found)
                {
                    Person foundPerson = dbContext.People.SingleOrDefault(p => p.JMBG.Equals(personToDelete.JMBG));
                    /*
                    if(dbContext.Accounts.Any(a => a.PersonWithAccount.JMBG.Equals(foundPerson.JMBG)))
                    {
                        Account foundAccount = dbContext.Accounts.SingleOrDefault(a => a.PersonWithAccount.JMBG.Equals(foundPerson.JMBG));
                        DeleteAccount(foundAccount);
                    }
                    */
                    dbContext.People.Remove(foundPerson);
                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public Person GetSinglePerson(string jmbg)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.People.FirstOrDefault(p => p.JMBG.Equals(jmbg));
            }
        }

        public List<Person> GetAllPeople()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                List<Person> people = dbContext.People.ToList();
                if (people == null)
                {
                    people = new List<Person>();
                }

                return people;
            }
        }
        #endregion

        #region Account
        public bool AddAccount(Account accountToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Accounts.Any(a => a.Username.Equals(accountToAdd.Username));
                if (found)
                {
                    return false;
                }

                /*
                if(!dbContext.People.Any(p => p.JMBG.Equals(accountToAdd.PersonWithAccount.JMBG)))
                {
                    if(!AddPerson(accountToAdd.PersonWithAccount))
                    {
                        return false;
                    }
                }
                */

                dbContext.Accounts.Add(accountToAdd);
                dbContext.SaveChanges();
                return true;
            }
        }

        /*
        public bool AddAccount(Account accountToAdd, Person personToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Accounts.Any(a => a.Username.Equals(accountToAdd.Username));
                if (found)
                {
                    return false;
                }

                //if(!AddPerson(personToAdd))
                //{
                //    return false;
                //}

                dbContext.Accounts.Add(accountToAdd);
                dbContext.SaveChanges();
                return true;
            }
        }
        */

        public bool ModifyAccount(Account accountToModify)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Accounts.Any(a => a.Username.Equals(accountToModify.Username));
                if (found)
                {
                    Account foundAccount = dbContext.Accounts.SingleOrDefault(a => a.Username.Equals(accountToModify.Username));
                    dbContext.Accounts.Attach(foundAccount);

                    foundAccount.Password = accountToModify.Password;
                    foundAccount.Role = accountToModify.Role;
                    foundAccount.FirstName = accountToModify.FirstName;
                    foundAccount.LastName = accountToModify.LastName;

                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool DeleteAccount(Account accountToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Accounts.Any(a => a.Username.Equals(accountToDelete.Username));
                if (found)
                {
                    Account foundAccount = dbContext.Accounts.SingleOrDefault(a => a.Username.Equals(accountToDelete.Username));
                    dbContext.Accounts.Remove(foundAccount);
                    dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public Account GetSingleAccountByUsername(String accountUsername)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                return dbContext.Accounts.FirstOrDefault(p => p.Username.Equals(accountUsername));
            }
        }

        public List<Account> GetAllAccounts()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                List<Account> accounts = dbContext.Accounts.ToList();
                if (accounts == null)
                {
                    accounts = new List<Account>();
                }

                return accounts;
            }
        }
        #endregion

        #region Event
        public bool AddEvent(Event eventToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Events.Any(e => e.EventId.Equals(eventToAdd.EventId));
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
                bool found = dbContext.Events.Any(p => p.EventId.Equals(eventToModify.EventId));
                if (found)
                {
                    Event foundEvent = dbContext.Events.SingleOrDefault(e => e.EventId.Equals(eventToModify.EventId));
                    dbContext.Events.Attach(foundEvent);

                    foundEvent.LastEditTimeStamp = eventToModify.LastEditTimeStamp;
                    foundEvent.LastEditTimeStamp = DateTime.Now;
                    foundEvent.ScheduledDateTimeBeging = eventToModify.ScheduledDateTimeBeging;
                    foundEvent.ScheduledDateTimeEnd = eventToModify.ScheduledDateTimeEnd;
                    foundEvent.EventTitle = eventToModify.EventTitle;
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
                bool found = dbContext.Events.Any(p => p.EventId.Equals(eventToDelete.EventId));
                if (found)
                {
                    Event foundEvent = dbContext.Events.SingleOrDefault(e => e.EventId.Equals(eventToDelete.EventId));
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
                return dbContext.Events.FirstOrDefault(e => e.EventId.Equals(eventId));
            }
        }

        public List<Event> GetAllEvents()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                List<Event> events = dbContext.Events.ToList();
                if (events == null)
                {
                    events = new List<Event>();
                }

                return events;
            }
        }
        #endregion
    }
}
