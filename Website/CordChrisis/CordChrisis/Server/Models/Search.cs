using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CordChrisis.Models
{
    public class Search
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public int ApiType { get; set; }
        public int Difficulty { get; set; }
        public double Ratings { get; set; }
        public int Plays { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
