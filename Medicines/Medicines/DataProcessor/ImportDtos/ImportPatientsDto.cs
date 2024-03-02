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
        [JsonProperty("FullName")]
        [MinLength(5)]
        [MaxLength(100)]
        [Required]
        public string FullName { get; set; } = null!;

        [JsonProperty("AgeGroup")]
        [Required]
        [Range(0, 2)]
        public int AgeGroup { get; set; }

        [JsonProperty("Gender")]
        [Required]
        [Range(0, 1)]
        public int Gender { get; set; }

        [JsonProperty("Medicines")]
        [Required]
        public int[] Medicines { get; set; }
    }
}
