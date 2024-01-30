using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType(nameof(District))]
    public class ImportDistrictDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression("^[A-Z]{2}-\\d{5}$")]
        [XmlElement(nameof(PostalCode))]
        public string PostalCode { get; set; } = null!;

        [Required]
        [XmlAttribute(nameof(Region))]
        public string Region { get; set; } = null!;

        [XmlArray(nameof(Properties))]
        [XmlArrayItem(nameof(Property))]
        public ImportPropertyDto[] Properties { get; set; } = null!;


    }
}
