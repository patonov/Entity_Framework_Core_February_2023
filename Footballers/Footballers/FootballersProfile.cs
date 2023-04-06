namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.DataProcessor.ImportDto;

    // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
    public class FootballersProfile : Profile
    {
        public FootballersProfile()
        {
            this.CreateMap<ImportFootballerDto, Footballer>();

            this.CreateMap<ImportCoachDto, Coach>()
                .ForSourceMember(s => s.Footballers, opt => opt.DoNotValidate());

            this.CreateMap<ImportTemDto, Team>()
                .ForSourceMember(s => s.Footballers, opt => opt.DoNotValidate());

            this.CreateMap<Footballer, ExportFootballerDto>()
                .ForMember(d => d.Name, s => s.MapFrom(s => s.Name))
                .ForMember(d => d.Position, s => s.MapFrom(s => s.PositionType.ToString()));

            this.CreateMap<Coach, ExportCoachDto>()
                .ForMember(d => d.Name, s => s.MapFrom(s => s.Name))
                .ForMember(d => d.FootballersCount, s => s.MapFrom(s => s.Footballers.Count))
                .ForMember(d => d.Footballers, s => s.MapFrom(s => s.Footballers.ToArray().OrderBy(f => f.Name)));

        }
    }
}
