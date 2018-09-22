using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels.PersonViewModels
{
    class DeletePersonConfirmationViewModel
    {
        public Person PersonToBeDeleted { get; set; }

        public DeletePersonConfirmationViewModel(Person personToBeDeleted)
        {
            PersonToBeDeleted = personToBeDeleted;
        }
    }
}
