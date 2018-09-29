using Common.BaseObserverPattern;
using Common.Helpers;
using Common.IAccess;
using Common.IModels;
using Common.Models;
using Server.Providers.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private DbManager()
        {
            _eventNotifier = new EventNotifier();
            GetAllPeople().ForEach(p => _eventNotifier.RegisterObserver(new PersonObserver(p)));
        }
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

        private EventNotifier _eventNotifier;

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

                _eventNotifier.RegisterObserver(new PersonObserver(personToAdd));
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
                    IObserverPattern personObserver = _eventNotifier.Observers.FirstOrDefault(p => ((Person)p).JMBG.Equals(personToModify.JMBG));
                    _eventNotifier.UnregisterObserver(personObserver);
                    _eventNotifier.RegisterObserver(new PersonObserver(personToModify));
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

                    IObserverPattern foundObserver = _eventNotifier.Observers.FirstOrDefault(p => ((Person)p).JMBG.Equals(personToDelete.JMBG));
                    _eventNotifier.UnregisterObserver(foundObserver);
                    return true;
                }

                return false;
            }
        }
        
        /*
        public Person GetSinglePerson(string jmbg)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Person foundPerson = dbContext.People.FirstOrDefault(p => p.JMBG.Equals(jmbg));
                return foundPerson;
            }
        }
        */
        public Person GetSinglePerson(string jmbg)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Person foundPerson = dbContext.People
                                              .Include(p => p.ScheduledEvents)
                                              .FirstOrDefault(p => p.JMBG.Equals(jmbg));
                return foundPerson;
            }
        }

        /*
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
        */
        public List<Person> GetAllPeople()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                List<Person> people = dbContext.People
                                               .Include(p => p.ScheduledEvents)
                                               .ToList();
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
        public Event AddEvent(Event eventToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Events.Any(e => e.EventId.Equals(eventToAdd.EventId));
                if (found)
                {
                    return null;
                }

                List<String> ParticipantsIds = new List<string>(eventToAdd.Participants.Select(p => p.JMBG));
                List<Person> SelectedParticipants = new List<Person>(eventToAdd.Participants);

                eventToAdd.EmptyTheListOfParticipants();
                foreach(String pId in ParticipantsIds)
                {
                    Person participant = dbContext.People.FirstOrDefault(p => p.JMBG.Equals(pId));
                    if(participant == null)
                    {
                        participant = new Person(SelectedParticipants.FirstOrDefault(p => p.JMBG.Equals(pId)));
                    }

                    dbContext.People.Attach(participant);
                    eventToAdd.AddParticipant(participant);
                }

                Event addedEvent = dbContext.Events.Add(eventToAdd);
                dbContext.SaveChanges();

                _eventNotifier.EvetToBeNotifiedAbout = addedEvent;
                _eventNotifier.NotifyAllAdditon();
                return addedEvent;
            }
        }

        public Event ModifyEvent(Event eventToModify)
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

                    dbContext.SaveChanges();

                    List<String> ParticipantsIds = new List<string>(eventToModify.Participants.Select(p => p.JMBG));
                    List<Person> SelectedParticipants = new List<Person>(eventToModify.Participants);

                    foundEvent.Participants = new List<Person>();
                    foreach (String pId in ParticipantsIds)
                    {
                        Person participant = dbContext.People.Include(p => p.ScheduledEvents).FirstOrDefault(p => p.JMBG.Equals(pId));
                        if (participant == null)
                        {
                            participant = new Person(SelectedParticipants.FirstOrDefault(p => p.JMBG.Equals(pId)));
                        }

                        dbContext.People.Attach(participant);

                        participant.ScheduledEvents.ForEach(e => e.Participants = new List<Person>());

                        dbContext.Events.Attach(foundEvent); //TREBA LI ATTACH
                       
                        foundEvent.AddParticipant(participant);
                       
                        dbContext.SaveChanges();                //ORGANIZYJ SAVE CHANGES
                    }

                    _eventNotifier.EvetToBeNotifiedAbout = foundEvent;
                    _eventNotifier.NotifyAllChange();
                    return foundEvent;
                }

                return null;
            }
        }

        public Event DeleteEvent(Event eventToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Events.Any(p => p.EventId.Equals(eventToDelete.EventId));
                if (found)
                {
                    Event foundEvent = dbContext.Events.SingleOrDefault(e => e.EventId.Equals(eventToDelete.EventId));
                    Event removedEvent = dbContext.Events.Remove(foundEvent);
                    dbContext.SaveChanges();

                    _eventNotifier.EvetToBeNotifiedAbout = foundEvent;
                    _eventNotifier.NotifyAllRemoval();
                    return removedEvent;
                }

                return null;
            }
        }

        /*
        public Event GetSingleEvent(Int32 eventId)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Event foundEvent = dbContext.Events.FirstOrDefault(e => e.EventId.Equals(eventId));
                return foundEvent;
            }
        }
        */
        public Event GetSingleEvent(Int32 eventId)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                Event foundEvent = dbContext.Events
                                            .Include(e => e.Participants)
                                            .FirstOrDefault(e => e.EventId.Equals(eventId));
                return foundEvent;
            }
        }
       
        /*
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
        */
        public List<Event> GetAllEvents()
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                List<Event> events = dbContext.Events
                                              .Include(e => e.Participants)
                                              .ToList();
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
