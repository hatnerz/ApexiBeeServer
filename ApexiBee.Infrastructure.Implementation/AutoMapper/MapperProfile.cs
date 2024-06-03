using ApexiBee.Application.DTO.Apiary;
using ApexiBee.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<HubStation, HubCreatedResult>();
            CreateMap<Apiary, ApiaryCreatedResult>()
                .ForMember(ac => ac.Hub, r => r.MapFrom(a => a.Hub));
        }
    }
}
