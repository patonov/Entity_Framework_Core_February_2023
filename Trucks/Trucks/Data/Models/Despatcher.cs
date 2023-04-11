using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        
        public string? Position { get; set; }

        public virtual ICollection<Truck> Trucks { get; set; } = new List<Truck>();


    }
}
