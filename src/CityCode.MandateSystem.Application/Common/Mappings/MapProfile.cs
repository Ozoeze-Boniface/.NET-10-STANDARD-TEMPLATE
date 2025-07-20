using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Commands;

namespace CityCode.MandateSystem.Application.Common.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CreateMandateCommand, MandateRequest>().ReverseMap();
            
            CreateMap<MandateRequest, Mandate>()
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}