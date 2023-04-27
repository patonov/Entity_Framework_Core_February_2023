using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Theatre.Data.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; }

        public float Rating { get; set; } 

        [Required]
        public virtual Genre Genre { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Screenwriter { get; set; } = null!;

        public virtual ICollection<Cast> Casts { get; set; } = new List<Cast>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();



    }
}
