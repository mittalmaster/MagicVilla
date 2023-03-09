using AutoMapper;
using MagicVilla_Web.Models.Dtos;

namespace MagicVilla_Web
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaCreateDTO,VillaDto>().ReverseMap();
            CreateMap<VillaUpdateDTO,VillaDto>().ReverseMap();
            CreateMap<VillaNumberCreateDTO,VillaNumberDTO > ().ReverseMap(); 
            CreateMap<VillaNumberUpdateDTO,VillaNumberDTO>().ReverseMap();            
        }

    }
}
