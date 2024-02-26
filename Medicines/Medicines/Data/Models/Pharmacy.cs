using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(14)]
        [RegularExpression("\\([1][2][3]\\)\\ [0-9]{3}\\-[0-9]{4}")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public bool IsNonStop { get; set; }

        [Required]
        public ICollection<Medicine> Medicines { get; set; } = null!;

    }
}
