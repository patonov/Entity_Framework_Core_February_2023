namespace Boardgames
{
    using AutoMapper;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ExportDto;

    public class BoardgamesProfile : Profile
    {
        // DO NOT CHANGE OR RENAME THIS CLASS!
        public BoardgamesProfile()
        {
            this.CreateMap<Boardgame, ExportBoardgameDto>()
            .ForMember(d => d.Name, s => s.MapFrom(s => s.Name))
            .ForMember(d => d.YearPublished, s => s.MapFrom(s => s.YearPublished));

            this.CreateMap<Creator, ExportCreatorDto>()
                .ForMember(d => d.Name, s => s.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.Count, s => s.MapFrom(s => s.Boardgames.Count))
                .ForMember(d => d.Boardgames, s => s.MapFrom(s => s.Boardgames.ToArray().OrderBy(b => b.Name)));


        }
    }
}