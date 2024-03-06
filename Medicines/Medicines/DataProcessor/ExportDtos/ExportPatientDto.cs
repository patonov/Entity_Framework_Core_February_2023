﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlAttribute("Gender")]
        public string Gender { get; set; }

        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; }

        [XmlArray("Medicines")]
        public ExportMedicinesDto[] Medicines { get; set; }
    }
}
