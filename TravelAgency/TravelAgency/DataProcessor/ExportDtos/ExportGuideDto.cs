﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class ExportGuideDto
    {
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;

        [XmlArray("TourPackages")]
        public ExportTourPacageDto[] TourPackages { get; set; }
    }
}
