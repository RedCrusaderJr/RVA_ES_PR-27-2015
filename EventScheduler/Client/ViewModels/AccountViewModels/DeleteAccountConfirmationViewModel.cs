using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels.AccountViewModels
{
    class DeleteAccountConfirmationViewModel
    {
        public Account AccountToDelete { get; set; }

        public DeleteAccountConfirmationViewModel(Account accountToDelete)
        {
            AccountToDelete = accountToDelete;
        }
    }
}
