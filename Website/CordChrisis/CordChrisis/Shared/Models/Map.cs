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
        public string JSON { get; set; }
        [Required]
        [MaxLength(25)]
        public string Genre { get; set; }
        [Required]
        public int BPM { get; set; }
        [Required]
        public int ApiType { get; set; }
        [Required]
        public Guid ThumbnailPK { get; set; }
        [Required]
        public int Difficulty { get; set; }
        [Required]
        public decimal Rating { get; set; }
        [Required]
        public int NumOfRatings { get; set; }
        public double MapRating => (double)Rating;
        [Required]
        public int Plays { get; set; }
        [Required]
        public  DateTime CreatedDate { get; set; }
        [Required]
        public bool PublicVisible { get; set; }
        [Required]
        [MaxLength(25)]
        public string Author { get; set; }
        [MaxLength(50)]
        public string AuthorID { get; set; }
    }
}
