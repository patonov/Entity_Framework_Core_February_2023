namespace Footballers
{
    using AutoMapper;
    using Footballers.Data.Models;
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
            

        }
    }
}
