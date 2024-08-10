using NetPay.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ImportDtos
{
    [XmlType(nameof(Household))]
    public class ImportHouseholdDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [XmlElement(nameof(ContactPerson))]
        public string ContactPerson { get; set; } = null!;

        [MinLength(6)]
        [MaxLength(80)]
        [XmlElement(nameof(Email))]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]
        [XmlAttribute("phone")]
        public string PhoneNumber { get; set; } = null!;
    }
}
