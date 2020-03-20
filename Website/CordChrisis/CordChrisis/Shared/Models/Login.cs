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
<<<<<<< HEAD
        [Required]
        public string Email { get; set; }
        [Key]
        [Required]
        public SecureString Password { get; set; }
        [Required]
        public Guid ID { get; set; }

=======
        
        [Required]
        public string Email { get; set; }

        [Key]
        [Required]
        public string Password { get; set; }

    
        [Required]
        public Guid ID { get; set;}
>>>>>>> 0bb5e56fd55c5ff0c960c60de0945c2d6ff2f6ec

    }
}
