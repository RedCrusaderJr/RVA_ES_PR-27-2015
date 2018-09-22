using Common.IModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PersonWithAccount : Person, IAccount
    {
        #region Fields
        private String _username;
        private String _password;
        private ERole _role;
        #endregion

        #region Properties
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        [Required]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        [Required]
        public ERole Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged("Role");
            }
        }
        #endregion

        public PersonWithAccount() : base() { }

        public PersonWithAccount(String username) : base()
        {
            Username = username;
        }

        public PersonWithAccount(String username, String password, ERole role) : base()
        {
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
