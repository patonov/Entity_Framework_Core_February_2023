using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Theatre.Data.Models;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDto
    {

        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        [XmlElement("Title")]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; } = null!;

        [Required]
        [Range(0.00, 10.00)]
        [XmlElement("Raiting")]
        public float Rating { get; set; } 

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; } = null!;

        [Required]
        [MaxLength(700)]
        [XmlElement("Description")]
        public string Description { get; set; } = null!;

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        [XmlElement("Screenwriter")]
        public string Screenwriter { get; set; } = null!;

    }
}
