using Common.Contracts;
using Common.Helpers;
using Common.Models;
using log4net;
using Server.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    //TODO: FASADA
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class PersonServices_Provider : IPersonServices
    {

        private static readonly ILog logger = Log4netHelper.GetLogger();
        public Person AddPerson(Person person)
        {
            Person addedPerson = DbManager.Instance.AddPerson(person);
            if (addedPerson != null)
            {
                IPersonServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IPersonServicesCallback>();
                PersonServiceCallback.Instance.NotifyAllPersonAddition(addedPerson, currentCallbackProxy);

                addedPerson.ScheduledEvents = new List<Event>();
                return addedPerson;
            }

            return null;
        }

        public Person ModifyPerson(Person person)
        {
            Person modifiedPerson = DbManager.Instance.ModifyPerson(person);
            if (modifiedPerson != null)
            {
                IPersonServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IPersonServicesCallback>();
                PersonServiceCallback.Instance.NotifyAllPersonModification(modifiedPerson, currentCallbackProxy);

                modifiedPerson.ScheduledEvents = new List<Event>();
                return modifiedPerson;
            }

            return null;
        }

        public Person DeletePerson(Person person)
        {
            Person deletedPerson = DbManager.Instance.DeletePerson(person);
            if (deletedPerson != null)
            {
                IPersonServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IPersonServicesCallback>();
                PersonServiceCallback.Instance.NotifyAllPersonRemoval(deletedPerson, currentCallbackProxy);

                deletedPerson.ScheduledEvents = new List<Event>();
                return deletedPerson;
            }

            return null;
        }

        public Person GetSinglePerson(string jmbg)
        {
            Person person = DbManager.Instance.GetSinglePerson(jmbg);

            if (person == null)
            {
                return null;
            }

            person.ScheduledEvents.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents.ForEach(ev => ev.Participants = new List<Person>())));
            return person;
        }

        public List<Person> GetAllPeople()
        {
            List<Person> people = DbManager.Instance.GetAllPeople();
            people.ForEach(p => p.ScheduledEvents.ForEach(e => e.Participants.ForEach(pa => pa.ScheduledEvents.ForEach(ev => ev.Participants = new List<Person>()))));
            return people;
        }

        public void Subscribe()
        {
            IPersonServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IPersonServicesCallback>();
            PersonServiceCallback.Instance.Subscribe(currentCallbackProxy);
        }

        public void Unsubscribe()
        {
            IPersonServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IPersonServicesCallback>();
            PersonServiceCallback.Instance.Unsubscribe(currentCallbackProxy);
        }

        public Person DuplicatePerson(Person person)
        {
            Person personToDuplicate = person.Duplicate() as Person;
            List<Event> eventsToSchedule = new List<Event>(personToDuplicate.ScheduledEvents);
            personToDuplicate.ScheduledEvents = new List<Event>();

            Person duplicatedPerson = DbManager.Instance.AddPerson(personToDuplicate);
            if(duplicatedPerson != null)
            { 
                duplicatedPerson.ScheduledEvents = new List<Event>(eventsToSchedule);

                Person successfullyDuplicatedPerson = DbManager.Instance.ModifyPerson(duplicatedPerson);
                if(successfullyDuplicatedPerson != null)
                {
                    IPersonServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IPersonServicesCallback>();
                    PersonServiceCallback.Instance.NotifyAllPersonDuplication(duplicatedPerson, currentCallbackProxy);

                    successfullyDuplicatedPerson.ScheduledEvents = new List<Event>();
                    return successfullyDuplicatedPerson;
                }
            }
            return null;
        }
    }
}
