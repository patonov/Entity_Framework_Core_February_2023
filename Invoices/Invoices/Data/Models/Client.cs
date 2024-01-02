﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string NumberVat { get; set; } = null!;

        [Required]
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        [Required]
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

        [Required]
        public virtual ICollection<ProductClient> ProductsClients { get; set; } = new List<ProductClient>();

    }
}
