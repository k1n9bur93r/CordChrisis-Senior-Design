using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Models
{
    public class UserLogin
    {
        public string Email { get; set; }
        SecureString Password { get; set; }


    }
}
