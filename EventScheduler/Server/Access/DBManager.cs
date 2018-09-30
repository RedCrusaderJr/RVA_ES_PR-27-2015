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
        public Person AddPerson(Person personToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.JMBG.Equals(personToAdd.JMBG));
                if (found)
                {
                    return null;
                }

                Person addedPerson = dbContext.People.Add(personToAdd);
                dbContext.SaveChanges();

                _eventNotifier.RegisterObserver(new PersonObserver(addedPerson));
                return addedPerson;
            }
        }

        public Person ModifyPerson(Person personToModify)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.JMBG.Equals(personToModify.JMBG));
                if (found)
                {
                    Person foundPerson = dbContext.People.Include(p => p.ScheduledEvents).SingleOrDefault(p => p.JMBG.Equals(personToModify.JMBG));
                    dbContext.People.Attach(foundPerson);

                    foundPerson.JMBG = personToModify.JMBG;
                    foundPerson.FirstName = personToModify.FirstName;
                    foundPerson.LastName = personToModify.LastName;
                    foundPerson.LastEditTimeStamp = DateTime.Now;

                    foreach (Event e in personToModify.ScheduledEvents)
                    {
                        Event foundEvent = dbContext.Events.FirstOrDefault(ev => ev.EventId.Equals(e.EventId));
                        if(foundEvent != null)
                        {
                            dbContext.Events.Attach(foundEvent);
                            foundEvent.Participants = new List<Person>();

                            if(foundPerson.ScheduledEvents.Contains(foundEvent, new EventComparer()))
                            {
                                Event eventInPerson = foundPerson.ScheduledEvents.FirstOrDefault(eve => eve.EventId.Equals(foundEvent.EventId));
                                foundPerson.ScheduledEvents.Remove(eventInPerson);
                                foundPerson.ScheduledEvents.Add(foundEvent);
                            }
                            else
                            {
                                foundPerson.ScheduledEvents.Add(foundEvent);
                            }
                        }
                        else
                        {
                            e.Participants = new List<Person>();
                            foundPerson.ScheduledEvents.Add(e);
                        }
                    }

                    dbContext.SaveChanges();

                    //OVDE NEGDE NOTIFIKACIJA ZA POTENCIJALNI KONFLIKT
                    IObserverPattern personObserver = _eventNotifier.Observers.FirstOrDefault(p => ((Person)p).JMBG.Equals(personToModify.JMBG));
                    _eventNotifier.UnregisterObserver(personObserver);
                    _eventNotifier.RegisterObserver(new PersonObserver(foundPerson));
                    return foundPerson;
                }

                return null;
            }
        }

        public Person DeletePerson(Person personToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.People.Any(p => p.JMBG.Equals(personToDelete.JMBG));
                if (found)
                {
                    Person foundPerson = dbContext.People.SingleOrDefault(p => p.JMBG.Equals(personToDelete.JMBG));
                    
                    Person deletedPerson = dbContext.People.Remove(foundPerson);
                    dbContext.SaveChanges();

                    IObserverPattern foundObserver = _eventNotifier.Observers.FirstOrDefault(p => ((Person)p).JMBG.Equals(personToDelete.JMBG));
                    _eventNotifier.UnregisterObserver(foundObserver);

                    return deletedPerson;
                }

                return null;
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
        public Account AddAccount(Account accountToAdd)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Accounts.Any(a => a.Username.Equals(accountToAdd.Username));
                if (found)
                {
                    return null;
                }

                Account addedAccount = dbContext.Accounts.Add(accountToAdd);
                dbContext.SaveChanges();
                return addedAccount;
            }
        }

        public Account ModifyAccount(Account accountToModify)
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
                    return foundAccount;
                }

                return null;
            }
        }

        public Account DeleteAccount(Account accountToDelete)
        {
            using (EventSchedulerContext dbContext = new EventSchedulerContext())
            {
                bool found = dbContext.Accounts.Any(a => a.Username.Equals(accountToDelete.Username));
                if (found)
                {
                    Account foundAccount = dbContext.Accounts.SingleOrDefault(a => a.Username.Equals(accountToDelete.Username));
                    Account deletedAccount = dbContext.Accounts.Remove(foundAccount);
                    dbContext.SaveChanges();

                    return deletedAccount;
                }

                return null;
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

                Event eventActualyGoingToBeAdded = new Event()
                {
                    Description = eventToAdd.Description,
                    EventTitle = eventToAdd.EventTitle,
                    ScheduledDateTimeBeging = eventToAdd.ScheduledDateTimeBeging,
                    ScheduledDateTimeEnd = eventToAdd.ScheduledDateTimeEnd,
                    LastEditTimeStamp = DateTime.Now
                };
                foreach (Person p in eventToAdd.Participants)
                {
                    Person foundPerson = dbContext.People.FirstOrDefault(per => per.JMBG.Equals(p.JMBG));
                    if(foundPerson != null)
                    {
                        dbContext.People.Attach(foundPerson);
                        foundPerson.ScheduledEvents = new List<Event>();
                        eventActualyGoingToBeAdded.Participants.Add(foundPerson);
                    }
                    else
                    {
                        p.ScheduledEvents = new List<Event>();
                        eventActualyGoingToBeAdded.Participants.Add(p);
                    }
                }
                Event addedEvent = dbContext.Events.Add(eventActualyGoingToBeAdded);
                dbContext.SaveChanges();

                {
                    /*
                    List<String> ParticipantsIds = new List<string>(eventToAdd.Participants.Select(p => p.JMBG));
                    List<Person> SelectedParticipants = new List<Person>(eventToAdd.Participants);

                    eventToAdd.EmptyTheListOfParticipants();
                    foreach(String pId in ParticipantsIds)
                    {
                        //Include(p => p.ScheduledEvents).
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
                    */
                }
                _eventNotifier.EvetToBeNotifiedAbout = addedEvent;
                _eventNotifier.NotifyAllAdditon();

                //Event evt = dbContext.Events.Include(e => e.Participants).FirstOrDefault(e => e.EventId.Equals(addedEvent.EventId));
                /*List<Person> updatedPeople = new List<Person>();
                foreach(Person p in addedEvent.Participants)
                {
                    updatedPeople.Add(dbContext.People.First(per => per.JMBG.Equals(p.JMBG)));
                }
                addedEvent.Participants = new List<Person>(updatedPeople);*/
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
                    Event foundEvent = dbContext.Events.Include(e => e.Participants).SingleOrDefault(e => e.EventId.Equals(eventToModify.EventId));
                    dbContext.Events.Attach(foundEvent);

                    foundEvent.LastEditTimeStamp = eventToModify.LastEditTimeStamp;
                    foundEvent.LastEditTimeStamp = DateTime.Now;
                    foundEvent.ScheduledDateTimeBeging = eventToModify.ScheduledDateTimeBeging;
                    foundEvent.ScheduledDateTimeEnd = eventToModify.ScheduledDateTimeEnd;
                    foundEvent.EventTitle = eventToModify.EventTitle;
                    foundEvent.Description = eventToModify.Description;

                    foreach(Person p in eventToModify.Participants)
                    {
                        if(!foundEvent.Participants.Contains(p, new PersonComparer()))
                        {
                            Person foundPerson = dbContext.People.FirstOrDefault(per => per.JMBG.Equals(p.JMBG));
                            dbContext.People.Attach(foundPerson);
                            foundEvent.Participants.Add(foundPerson);
                        }
                        else
                        {
                            Person foundPerson = foundEvent.Participants.FirstOrDefault(per => per.JMBG.Equals(p.JMBG));
                            dbContext.People.Attach(foundPerson);
                            foundEvent.Participants.Remove(foundPerson);
                            Person newFoundPerson = dbContext.People.FirstOrDefault(pers => pers.JMBG.Equals(p.JMBG));
                            dbContext.People.Attach(newFoundPerson);
                            foundEvent.Participants.Add(newFoundPerson);
                        }
                    }
                    dbContext.SaveChanges();

                    _eventNotifier.EvetToBeNotifiedAbout = foundEvent;
                    _eventNotifier.NotifyAllChange();

                    /*
                    List<Person> updatedPeople = new List<Person>();
                    foreach (Person p in foundEvent.Participants)
                    {
                        updatedPeople.Add(dbContext.People.First(per => per.JMBG.Equals(p.JMBG)));
                    }
                    foundEvent.Participants = new List<Person>(updatedPeople);
                    */
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

                    List<Person> updatedPeople = new List<Person>();
                    foreach (Person p in removedEvent.Participants)
                    {
                        updatedPeople.Add(dbContext.People.First(per => per.JMBG.Equals(p.JMBG)));
                    }
                    removedEvent.Participants = new List<Person>(updatedPeople);
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
