using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Shared.Models
{
    public class Create
    {

    
        [Required]
        public Guid ID { get; set;}

        public int UserID
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please provide username", AllowEmptyStrings = false)]
        public string Email
        {
            get;
            set;
        }
        [Key]
        [Required(ErrorMessage = "Please provide Password", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be 8 char long.")]
        public string Password
        {
            get;
            set;
        }
        [Compare("Password", ErrorMessage = "Confirm password does not match.")]
        public string ConfirmPassword
        {
            get;
            set;
        }
   

    }
}
