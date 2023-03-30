using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Export
{
    public class ExportCarPriceDto
    {

        public string Name { get; set; } = null!;

        public int CarsBought { get; set; }

        public decimal[] CarPrices { get; set; } = null!;
    }
}
