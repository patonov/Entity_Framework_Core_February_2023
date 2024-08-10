using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPay.DataProcessor.ExportDtos
{
    [JsonObject("Supplier")]
    public class ExportSupplierName
    {
        [JsonProperty("SupplierName")]
        public string SupplierName { get; set; } = null!;


    }
}
