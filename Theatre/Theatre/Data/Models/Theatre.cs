using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        [Key]
        public int Id { get; set; }

        [MinLength(4)]
        [MaxLength(30)]
        [Required]
        public string Name { get; set; } = null!;

        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        [MinLength(4)]
        [MaxLength(30)]
        [Required]
        public string Director { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();


    }
}
