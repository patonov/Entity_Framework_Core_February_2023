using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(16)]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }

        [MaxLength(500)]
        [MinLength(5)]
        public string Details { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(5)]
        public string Address { get; set; } = null!;

        [Required]
        public DateTime DateOfAcquisition { get; set; }

        [Required]
        [ForeignKey(nameof(District))]
        public int DistrictId { get; set; }
        public virtual District District { get; set; } = null!;

        [Required]
        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new List<PropertyCitizen>();


    }
}
