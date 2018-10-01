using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract(CallbackContract = typeof(IPersonServicesCallback))]
    public interface IPersonServices
    {
        [OperationContract]
        Person AddPerson(Person person);

        [OperationContract]
        Person DuplicatePerson(Person person);

        [OperationContract]
        Person ModifyPerson(Person person);

        [OperationContract]
        Person DeletePerson(Person person);

        [OperationContract]
        Person GetSinglePerson(String jmbg);

        [OperationContract]
        List<Person> GetAllPeople();

        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();
    }

    public interface IPersonServicesCallback
    {
        [OperationContract]
        void NotifyPersonAddition(Person addedPerson);

        [OperationContract]
        void NotifyPersonDuplicate(Person addedPerson);

        [OperationContract]
        void NotifyPersonRemoval(Person removedPerson);

        [OperationContract]
        void NotifyPersonModification(Person modifiedPerson);
    }
}
