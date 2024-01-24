using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    using Cadastre.Data.Enumerations;
    using MagicDigits;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class District
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        [MinLength(MagicalSpells.MinNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression("/[A-Z]{2}\\-[0-9]{5}/gm")]
        public string PostalCode { get; set; } = null!;

        [Required]
        [Range(0, 3)]
        public Region Region { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
