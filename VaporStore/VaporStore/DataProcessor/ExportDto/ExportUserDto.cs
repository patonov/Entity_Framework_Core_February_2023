using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlAttribute("username")]
        public string Username { get; set; } = null!;

        [XmlArray("Purchases")]
        public ExportPurchaseDto[] Purchases { get; set; } = null!;

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }
    }
}
