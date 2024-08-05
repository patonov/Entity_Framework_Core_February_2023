using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TravelAgency.Data.Models;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType(nameof(Customer))]
    public class ImportCustomerDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        [XmlElement("Email")]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression("(\\+[0-9]{12})")]
        [XmlAttribute("phoneNumber")]
        public string PhoneNumber { get; set; } = null!;
    }
}
