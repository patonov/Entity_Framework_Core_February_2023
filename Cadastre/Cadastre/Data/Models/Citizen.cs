using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    public class Citizen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string LastName { get; set; } = null!;

        [Required]
        public DataType BirthDate { get; set; }

        [Required]
        public virtual MaritalStatus MaritalStatus { get; set; }

        [Required]
        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new List<PropertyCitizen>();

    }
}
