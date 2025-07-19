using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Common.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<MandateRequest, Mandate>().ReverseMap();
        }
    }
}