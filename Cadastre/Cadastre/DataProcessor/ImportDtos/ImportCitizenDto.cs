using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ImportDtos
{
    [JsonObject(nameof(Citizen))]
    public class ImportCitizenDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        [JsonProperty(nameof(FirstName))]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        [JsonProperty(nameof(LastName))]
        public string LastName { get; set; } = null!;

        [Required]
        [JsonProperty(nameof(BirthDate))]
        public string BirthDate { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(MaritalStatus))]
        [JsonProperty(nameof(MaritalStatus))]
        public string MaritalStatus { get; set; } = null!;

        [Required]
        [JsonProperty(nameof(Properties))]
        public int[] Properties { get; set; } = null!;

    }
}
