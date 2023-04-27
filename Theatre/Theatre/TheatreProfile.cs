namespace Theatre
{
    using AutoMapper;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;

    public class TheatreProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public TheatreProfile()
        {
            this.CreateMap<Cast, ExportCastDto>()
                .ForMember(d => d.FullName, s => s.MapFrom(s => s.FullName))
                .ForMember(d => d.MainCharacter, s => s.MapFrom(s => $"Plays main character in '{s.Play.Title}'."));

            this.CreateMap<Play, ExportPlayDto>()
                .ForMember(d => d.Title, s => s.MapFrom(s => s.Title))
                .ForMember(d => d.Duration, s => s.MapFrom(s => s.Duration.ToString("c")))
                .ForMember(d => d.Rating, s => s.MapFrom(s => s.Rating == 0 ? "Premier" : s.Rating.ToString()))
                .ForMember(d => d.Genre, s => s.MapFrom(s => s.Genre.ToString()))
                .ForMember(d => d.Actors, s => s.MapFrom(s => s.Casts.Where(c => c.IsMainCharacter)
                .OrderByDescending(c => c.FullName).ToArray()));
                
        }
    }
}
