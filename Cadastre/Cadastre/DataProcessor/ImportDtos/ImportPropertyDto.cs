using Cadastre.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType(nameof(Property))]
    public class ImportPropertyDto
    {
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        [XmlElement(nameof(PropertyIdentifier))]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        [XmlElement(nameof(Area))]
        public int Area { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        [XmlElement(nameof(Details))]
        public string? Details { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(5)]
        [XmlElement(nameof(Address))]
        public string Address { get; set; } = null!;

        [Required]
        [XmlElement(nameof(DateOfAcquisition))]
        public string DateOfAcquisition { get; set; } = null!;

    }
}
