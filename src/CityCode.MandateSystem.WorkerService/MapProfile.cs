using AutoMapper;

namespace CityCode.MandateSystem.WorkerService
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<MandateRequest, Mandate>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

            CreateMap<Mandate, MandateRequest>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
        }
    }
}
