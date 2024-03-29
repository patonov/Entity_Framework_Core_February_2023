﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Artillery.Data.Models
{
    public class Shell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(2, 1680)]
        public double ShellWeight { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; } = new List<Gun>();


    }
}
