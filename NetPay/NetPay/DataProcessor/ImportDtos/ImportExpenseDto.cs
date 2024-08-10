using NetPay.Data.Models.Enums;
using NetPay.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetPay.DataProcessor.ImportDtos
{
    [JsonObject("Expense")]
    public class ImportExpenseDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [JsonProperty("ExpenseName")]
        public string ExpenseName { get; set; } = null!;

        [Range(0.01, 100000)]
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }

        [Required]
        [JsonProperty("DueDate")]
        public string DueDate { get; set; } = null!;

        [Required]
        [JsonProperty("PaymentStatus")]
        public string PaymentStatus { get; set; } = null!;

        [JsonProperty("HouseholdId")]
        public int HouseholdId { get; set; }

        [JsonProperty("ServiceId")]
        public int ServiceId { get; set; }
        
    }
}
