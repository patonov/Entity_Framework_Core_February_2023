using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace VaporStore.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Username { get; set; } = null!;

        [Required]
        [RegularExpression(@"([A-Z][a-z]+)[ ]([A-Z][a-z]+)")]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Range(3, 103)]
        public int Age { get; set; }

        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    }
}
