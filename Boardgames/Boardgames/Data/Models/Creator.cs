using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Boardgames.Data.Models
{
    public class Creator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(7)]
        [MinLength(2)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(7)]
        [MinLength(2)]
        public string LastName { get; set; } = null!;

       public virtual ICollection<Boardgame> Boardgames { get; set; } = new List<Boardgame>();

    }
}
