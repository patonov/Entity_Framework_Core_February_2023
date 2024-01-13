using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientDto
    {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; } 

        [XmlElement("ClientName")]
        public string Name { get; set; } = null!;

        [XmlElement("VatNumber")]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Invoices")]
        public ExportInvoiceDto[] Invoices { get; set; } = null!;

    }
}
