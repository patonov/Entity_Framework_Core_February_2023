﻿using Artillery.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class Gun
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Manufacturer))]
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; } = null!;

        [Required]
        [Range(100, 1350000)]
        public int GunWeight { get; set; }

        [Required]
        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Required]
        [Range(1, 100000)]
        public int Range { get; set; }

        [Required]
        public virtual GunType GunType { get; set; }

        [Required]
        [ForeignKey(nameof(Shell))]
        public int ShellId { get; set; }
        public virtual Shell Shell { get; set; } = null!;

        public ICollection<CountryGun> CountriesGuns { get; set; } = new List<CountryGun>();

    }
}
