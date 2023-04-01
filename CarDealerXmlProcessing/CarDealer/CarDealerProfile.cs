using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();

            this.CreateMap<ImportPartDto, Part>()
                .ForMember(d => d.SupplierId, opt => opt.MapFrom(s => s.SupplierId.Value));

            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());

            this.CreateMap<ImportCustomerDto, Customer>();

            this.CreateMap<ImportSaleDto, Sale>();

            this.CreateMap<Part, ExportCarPartsDto>();

            this.CreateMap<Car, ExportCarDto>();

            this.CreateMap<Car, ExportCarBmwDto>();
            
            this.CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.PartsCars.Select(pc => pc.Part)
                .OrderByDescending(p => p.Price).ToArray()));

            
            this.CreateMap<Customer, ExportCarPriceDto>()
                .ForMember(dst => dst.CarsBought,
                    opt => opt.MapFrom(src => src.Sales.Count))
                .ForMember(dst => dst.CarPrices,
                    opt => opt.MapFrom(
                        src => src.IsYoungDriver == true ?
                            src.Sales.Select(s => (decimal)0.95 * s.Car.PartsCars
                                .Sum(pc => pc.Part.Price))
                            : src.Sales.Select(s => s.Car.PartsCars
                                .Sum(pc => pc.Part.Price))));

            this.CreateMap<ExportCarPriceDto, CustomersWithSalesDto>()
                .ForMember(dst => dst.MoneySpent,
                    opt => opt.MapFrom(
                        src => Math.Round(src.CarPrices.Sum())));


        }
    }
}
