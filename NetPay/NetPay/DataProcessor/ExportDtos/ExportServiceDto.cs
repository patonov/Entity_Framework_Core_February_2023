using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPay.DataProcessor.ExportDtos
{
    [JsonObject("Service")]
    public class ExportServiceDto
    {
        [JsonProperty("ServiceName")]
        public string ServiceName { get; set; } = null!;
               
        public ExportSupplierName[] Suppliers { get; set; }
    }
}
