using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Shared.Models
{
    public class Map
    {
        [Key]
        [Required]
        public Guid ID { get; set; }

        [MaxLength(100)]
        public Guid GroupID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2048)]
        public string URL { get; set; }
        [Required]
        [MaxLength(25)]
        public string Genre { get; set; }
        [Required]
        [MaxLength(3)]
        public int BPM { get; set; }
        [Required]
        [MaxLength(1)]
        public int ApiType { get; set; }
        [Required]
        public Guid ThumbnailPK { get; set; }
        [Required]
        [MaxLength(1)]
        public int Difficulty { get; set; }
        [Required]
        [MaxLength(10)]
        public decimal Rating { get; set; }
        [MaxLength(10)]
        public double MapRating => (double)Rating;
        [Required]
        public int Plays { get; set; }
        [Required]
        public  DateTime CreatedDate { get; set; }
        [Required]
        [MaxLength(1)]
        public bool PublicVisible { get; set; }
        [Required]
        [MaxLength(25)]
        public string Author { get; set; }
    }
}
