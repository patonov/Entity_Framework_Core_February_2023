using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace Trucks.Data.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression(@"[A-Z]{2}[0-9]{4}[A-Z]{2}")]
        public string RegistrationNumber { get; set; } = null!;
        
        [Required]
        [StringLength(17)]
        public string VinNumber { get; set; } = null!;

        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [Required]
        [Range(0, 3)]
        public virtual CategoryType CategoryType { get; set; }

        [Required]
        [Range(0, 4)]
        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }
        public virtual Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = new List<ClientTruck>();

    }
}
