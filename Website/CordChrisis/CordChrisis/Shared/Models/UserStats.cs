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

        public int TotalMapsMade { get; set; }

        public int HighestScore { get; set; }

        public int MaxCombo { get; set; }

        public int TotalScore { get; set; }

        public string MostPlayedMap { get; set; }

        public int MapsCreated { get; set; }

        public int TotalGamesPlayed { get; set; }

        public byte[] UserImage { get; set; }


    }
}
