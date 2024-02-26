using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        [MinLength(5)]
        public string FullName { get; set; } = null!;

        [Required]
        public virtual AgeGroup AgeGroup { get; set; }

        [Required]
        public virtual Gender Gender { get; set; }

        [Required]
        public ICollection<PatientMedicine> PatientsMedicines { get; set; } = null!;
    }
}
