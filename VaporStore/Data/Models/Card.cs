using Castle.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"\d{4}[ ]\d{4}[ ]\d{4}[ ]\d{4}")]
        public string Number { get; set; } = null!;

        [Required]
        [RegularExpression(@"\d{3}")]
        public string Cvc { get; set; } = null!;

        [Range(0, 1)]
        public virtual CardType Type { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

       public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
       

    }
}
