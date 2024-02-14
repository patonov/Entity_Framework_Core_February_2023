using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.ImportDto
{
    [JsonObject(nameof(Game))]
    public class ImportGameDto
    {
        [Required]
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [Range(0, Double.MaxValue)]
        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty(nameof(ReleaseDate))]
        public string ReleaseDate { get; set; } = null!;

        [Required]
        [JsonProperty(nameof(Developer))]
        public string Developer { get; set; } = null!;

        [Required]
        [JsonProperty(nameof(Genre))]
        public string Genre { get; set; } = null!;

        [JsonProperty(nameof(Tags))]
        public string[] Tags { get; set; } = null!;
    }
}
