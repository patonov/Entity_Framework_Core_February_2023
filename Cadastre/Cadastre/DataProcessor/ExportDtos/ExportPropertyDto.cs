using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    [XmlType("Property")]
    public class ExportPropertyDto
    {
        [XmlAttribute("postal-code")]
        public string PostalCode { get; set; } = null!;

        [XmlElement(nameof(PropertyIdentifier))]
        public string PropertyIdentifier { get; set; } = null!;

        [XmlElement(nameof(Area))]
        public int Area { get; set; }

        [XmlElement(nameof(Details))]
        public string Details { get; set; } = null!;

        [XmlElement(nameof(DateOfAcquisition))]
        public string DateOfAcquisition { get; set; } = null!;
    }
}
