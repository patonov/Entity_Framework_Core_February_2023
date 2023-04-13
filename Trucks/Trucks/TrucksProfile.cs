namespace Trucks
{
    using AutoMapper;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ExportDto;

    public class TrucksProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public TrucksProfile()
        {
            this.CreateMap<Truck, ExportTruckDto>()
                .ForMember(d => d.RegistrationNumber, s => s.MapFrom(s => s.RegistrationNumber))
                .ForMember(d => d.Make, s => s.MapFrom(s => s.MakeType.ToString()));

            this.CreateMap<Despatcher, ExportDespatcherDto>()
                    .ForMember(d => d.TrucksCount, s => s.MapFrom(s => s.Trucks.Count))
                    .ForMember(d => d.DespatcherName, s => s.MapFrom(s => s.Name))
                    .ForMember(d => d.Trucks, s => s.MapFrom(s => s.Trucks.ToArray().OrderBy(t => t.RegistrationNumber)));
        
        }
    }
}
