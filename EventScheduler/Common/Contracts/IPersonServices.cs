﻿using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IPersonServices
    {
        [OperationContract]
        bool AddPerson(Person person);

        [OperationContract]
        bool ModifyPerson(Person person);

        [OperationContract]
        bool DeletePerson(Person person);

        [OperationContract]
        Person GetSinglePerson(Int32 id);

        [OperationContract]
        List<Person> GetAllPeople();
    }
}