using CityCode.MandateSystem.Application.Commands;

namespace CityCode.MandateSystem.Application.Common.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CreateMandateCommand, MandateRequest>().ReverseMap().ForMember(dest => dest.Documents, opt => opt.Ignore());
            
            CreateMap<MandateRequest, Mandate>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<EditMandateCommand, MandateRequest>()
                .ForMember(dest => dest.MandateRequestId, opt => opt.Ignore()) // Don't update the Id
                .ForMember(dest => dest.MandateReference, opt => opt.Ignore()) // Don't change mandate reference
                .ForMember(dest => dest.BillerId, opt => opt.Ignore()) // Don't update biller/product info
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore()) // Don't update audit fields
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null values
        }
    }
}