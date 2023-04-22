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

        [MinLength(4)]
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(0.00, 10.00)]
        public float Rating { get; set; }

        [Required]
        public virtual Genre Genre { get; set; }

        [Required]
        [MinLength(700)]
        public string Description { get; set; } = null!;

        [MinLength(4)]
        [MaxLength(30)]
        [Required]
        public string Screenwriter { get; set; } = null!;

        public virtual ICollection<Cast> Casts { get; set; } = new List<Cast>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

}
}
