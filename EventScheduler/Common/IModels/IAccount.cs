using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IModels
{
    public interface IAccount
    {
        String Username {get; set;}
        String Password { get; set; }
        ERole Role { get; set; }
    }
}
