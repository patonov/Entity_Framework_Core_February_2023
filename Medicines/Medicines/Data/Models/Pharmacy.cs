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
        public Pharmacy()
        {
            this.Medicines = new List<Medicine>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public bool IsNonStop { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; }

    }
}
