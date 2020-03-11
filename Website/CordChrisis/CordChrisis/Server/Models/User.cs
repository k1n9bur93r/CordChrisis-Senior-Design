using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Models
{
    public class User
    {
        [Key]
        public string ID{ get; set; }
        [Required]
        [MaxLength(25)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(3)]
        public int Rank { get; set; }
        [Required]
        [MaxLength(1)]
        public bool IsDeleted { get; set; }
    }
}
