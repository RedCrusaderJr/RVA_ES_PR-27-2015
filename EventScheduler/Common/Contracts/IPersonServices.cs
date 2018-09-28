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
        bool AddPerson(Person person);

        [OperationContract]
        bool ModifyPerson(Person person);

        [OperationContract]
        bool DeletePerson(Person person);

        [OperationContract]
        Person GetSinglePerson(String jmbg);

        [OperationContract]
        List<Person> GetAllPeople();
    }

    public interface IPersonServicesCallback
    {
        [OperationContract]
        void NotifyAddEvent(Event addedEvent);

        [OperationContract]
        void NotifyDeleteEvent(Event removedEvent);

        [OperationContract]
        void NotifyModifyEvent(Event modifyedEvent);
    }
}
