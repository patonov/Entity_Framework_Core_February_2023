﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Artillery.Data.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        [MinLength(4)]
        public string CountryName { get; set; } = null!;

        [Required]
        [Range(50000, 10000000)]
        public int ArmySize { get; set; }

       public virtual ICollection<CountryGun> CountriesGuns { get; set; } = new List<CountryGun>();

    }
}
