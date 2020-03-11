using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Models
{
    public class UserMapStats
    {
        [Key]
        public string UserID { get; set; }
        [Required]
        [MaxLength(2048)]
        public string MapID { get; set; }
        [Required]
        [MaxLength(1)]
        public char LetterScore { get; set; }
        [Required]
        [MaxLength(5)]
        public int MaxCombo { get; set; }
        [Required]
        [MaxLength(10)]
        public int Score { get; set; }
        [Required]
        [MaxLength(2)]
        public double Rating { get; set; }
        [Required]
        [MaxLength(10)]
        public int Plays { get; set; }
    }
}
