﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPay.Data.Models
{
    public class Household
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string ContactPerson { get; set; } = null!;

        [MinLength(6)]
        [MaxLength(80)]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>(); 



    }
}
