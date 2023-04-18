namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ExportDto;
    using AutoMapper;

    public class ArtilleryProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public ArtilleryProfile()
        {
            this.CreateMap<Country, ExportCountryDto>()
                .ForMember(d => d.CountryName, s => s.MapFrom(s => s.CountryName))
                .ForMember(d => d.ArmySize, s => s.MapFrom(s => s.ArmySize));

            this.CreateMap<Gun, ExportGunDto>()
                .ForMember(d => d.Manufacturer, s => s.MapFrom(s => s.Manufacturer.ManufacturerName))
                .ForMember(d => d.GunType, s => s.MapFrom(s => s.GunType.ToString()))
                .ForMember(d => d.GunWeight, s => s.MapFrom(s => s.GunWeight))
                .ForMember(d => d.BarrelLength, s => s.MapFrom(s => s.BarrelLength))
                .ForMember(d => d.Range, s => s.MapFrom(s => s.Range))
                .ForMember(d => d.Countries, s => s.MapFrom(s => s.CountriesGuns.Where(s => s.Country.ArmySize > 4500000)
                .Select(cg => new ExportCountryDto() { CountryName = cg.Country.CountryName, ArmySize = cg.Country.ArmySize })
                .ToArray().OrderBy(cg => cg.ArmySize)));
        }
    }
}