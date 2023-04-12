﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        public int[] Trucks { get; set; }
    }
}
