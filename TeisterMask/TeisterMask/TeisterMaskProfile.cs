namespace TeisterMask
{
    using AutoMapper;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.Data.Models;

    public class TeisterMaskProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public TeisterMaskProfile()
        {
            this.CreateMap<Task, ExportTaskDto>()
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Name))
                .ForMember(d => d.LabelType, s => s.MapFrom(x => x.LabelType.ToString()));


            this.CreateMap<Project, ExportProjectDto>()
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Name))
                .ForMember(d => d.DueDate, s => s.MapFrom(x => x.DueDate.HasValue ? "Yes" : "No"))
                .ForMember(d => d.Count, s => s.MapFrom(x => x.Tasks.Count))
                .ForMember(d => d.Tasks, s => s.MapFrom(x => x.Tasks.ToArray().OrderBy(t => t.Name)));
        }
    }
}
