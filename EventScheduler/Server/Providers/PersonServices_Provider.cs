using Common.Contracts;
using Common.Helpers;
using Common.Models;
using Server.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    //TODO: da bi bila fasada mora da se ocisti
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class PersonServices_Provider : IPersonServices
    {
        public Person AddPerson(Person person)
        {
            Person addedPerson = DbManager.Instance.AddPerson(person);
            if (addedPerson != null)
            {
                IPersonServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IPersonServicesCallback>();
                PersonServiceCallback.Instance.NotifyAllPersonAddition(addedPerson, currentCallbackProxy);

                //POTENCIJALNO INF LOOP
                //addedPerson.ScheduledEvents.ForEach(e => e.Participants = GetAllPeople().Where(p => p.ScheduledEvents.Contains(e, new EventComparer())).ToList());
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

                //POTENCIJALNO INF LOOP
                //modifiedPerson.ScheduledEvents.ForEach(e => e.Participants = GetAllPeople().Where(p => p.ScheduledEvents.Contains(e, new EventComparer())).ToList());
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

                //POTENCIJALNO INF LOOP
                //deletedPerson.ScheduledEvents.ForEach(e => e.Participants = GetAllPeople().Where(p => p.ScheduledEvents.Contains(e, new EventComparer())).ToList());
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

            person.ScheduledEvents.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents = new List<Event>()));
            //person.ScheduledEvents.ForEach(e => e.Participants = new List<Person>());
            return person;
        }

        public List<Person> GetAllPeople()
        {
            List<Person> people = DbManager.Instance.GetAllPeople();
            people.ForEach(p => p.ScheduledEvents.ForEach(e => e.Participants.ForEach(pa => pa.ScheduledEvents = new List<Event>())));
            //people.ForEach(p => p.ScheduledEvents.ForEach(e => e.Participants = new List<Person>()));
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
    }
}
