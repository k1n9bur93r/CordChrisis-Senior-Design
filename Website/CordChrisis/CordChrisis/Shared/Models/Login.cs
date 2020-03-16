using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Shared.Models
{
    public class Login
    {
        public string Email { get; set; }
        public SecureString Password { get; set; }


    }
}
