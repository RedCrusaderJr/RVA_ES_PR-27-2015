using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IBasicOperations
    {
        [OperationContract]
        Person Login(string username, string password);

        [OperationContract]
        bool AddPerson(Person person);

        [OperationContract]
        bool ModifyPerson(Person person);
    }
}
