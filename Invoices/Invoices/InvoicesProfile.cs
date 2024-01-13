using AutoMapper;
using Invoices.Data.Models;
using Invoices.DataProcessor.ExportDto;

namespace Invoices
{
    public class InvoicesProfile : Profile
    {
        public InvoicesProfile()
        {
            this.CreateMap<Invoice, ExportInvoiceDto>()
                 .ForMember(d => d.Number, s => s.MapFrom(x => x.Number))
                 .ForMember(d => d.Amount, s => s.MapFrom(x => x.Amount))
                 .ForMember(d => d.DueDate, s => s.MapFrom(x => x.DueDate))
                 .ForMember(d => d.CurrencyType, s => s.MapFrom(x => x.CurrencyType));

            this.CreateMap<Client, ExportClientDto>()
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Name))
                .ForMember(d => d.NumberVat, s => s.MapFrom(x => x.NumberVat))
                .ForMember(d => d.InvoicesCount, s => s.MapFrom(x => x.Invoices.Count))
                .ForMember(d => d.Invoices, s => s.MapFrom(x => x.Invoices.ToArray()
                .OrderByDescending(i => i.IssueDate).ThenByDescending(i => i.DueDate)));
        }
    }
}
