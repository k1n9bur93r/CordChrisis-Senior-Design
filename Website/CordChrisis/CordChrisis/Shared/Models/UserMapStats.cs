using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Shared.Models
{
    public class UserMapStats
    {
        [Key]
        public string UserID { get; set; }

        public string Username { get; set; }

        [Required]
        [MaxLength(2048)]
        public string MapID { get; set; }

        public string LetterScore { get; set; }

        public int MaxCombo { get; set; }

        public int Score { get; set; }

        public decimal Rating { get; set; }

        public int Plays { get; set; }
    }
}
