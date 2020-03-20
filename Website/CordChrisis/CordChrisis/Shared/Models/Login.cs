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
        
        [Required]
        public string Email { get; set; }

        [Key]
        [Required]
        public string Password { get; set; }

    
        [Required]
        public Guid ID { get; set;}

    }
}
