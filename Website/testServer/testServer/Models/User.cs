﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace testServer.Models
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
    }
}