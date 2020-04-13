using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Shared.Models
{
    public class User
    {
        [Key]
        public string ID{ get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserEmail { get; set; }
        [Required]
        public int Rank { get; set; }
        [Required]
        public bool IsDeleted { get; set; }



    }
}
