using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientsDto
    {
        [Required]
        [MaxLength(150)]
        [MinLength(5)]
        [JsonProperty("FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [JsonProperty("AgeGroup")]
        public int AgeGroup { get; set; }

        [Required]
        [JsonProperty("Gender")]
        public int Gender { get; set; }

        [Required]
        [JsonProperty("Medicines")]
        public int[] Medicines { get; set; }
    }
}
