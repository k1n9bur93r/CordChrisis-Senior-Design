using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Shared.Models
{
    public class UserStats
    {
        [Key]
        public string ID { get; set; }
        [MaxLength(10)]
        public int TotalMapsMade { get; set; }
        [MaxLength(10)]
        public int HighestScore { get; set; }
        [MaxLength(10)]
        public int MaxCombo { get; set; }
        [MaxLength(100)]
        public int TotalScore { get; set; }
        [MaxLength(100)]
        public string MostPlayedMap { get; set; }
        [MaxLength(10)]
        public int MapsCreated { get; set; }
        [MaxLength(10)]
        public int TotalGamesPlayed { get; set; }

    }
}
