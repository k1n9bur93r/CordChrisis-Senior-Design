using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Shared.Models
{
    public class CreateAccount
    {
       public Login loginObject { get; set; }
       public string username { get; set; }

    }
}
