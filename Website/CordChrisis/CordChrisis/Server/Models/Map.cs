using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Models
{
    public class Map
    {
        [Key]
        public string ID { get; set; }

        [MaxLength(100)]
        public string GroupID { get; set; }

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
        public byte[] Thumbnail { get; set; }
        [Required]
        [MaxLength(1)]
        public int Difficulty { get; set; }
        [Required]
        [MaxLength(10)]
        public double Rating { get; set; }
        [Required]
        public int Plays { get; set; }
        [Required]
        DateTime CreatedDate { get; set; }
        [Required]
        [MaxLength(1)]
        public bool PublicVisible { get; set; }
        [Required]
        [MaxLength(25)]
        public string Author { get; set; }
    }
}
