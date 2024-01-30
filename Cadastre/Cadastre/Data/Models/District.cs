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
        public string Name { get; set; } = null!;

        [Required]
        public string PostalCode { get; set; } = null!;

        [Required]
        public virtual Region Region { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
