using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class PartCar
    {
        [ForeignKey("Part")]
        public int PartId { get; set; }
        public Part Part { get; set; } = null!;

        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; } = null!; 
    }
}
